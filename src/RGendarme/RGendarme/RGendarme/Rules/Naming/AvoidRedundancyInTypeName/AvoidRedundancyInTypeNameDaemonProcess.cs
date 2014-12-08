using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib.Extenstions;

namespace RGendarme.Rules.Naming.AvoidRedundancyInTypeName
{
    internal class AvoidRedundancyInTypeNameDaemonProcess : IDaemonStageProcess
    {
        private readonly IDaemonProcess _daemonProcess;

        public AvoidRedundancyInTypeNameDaemonProcess([NotNull]IDaemonProcess daemonProcess)
        {
            _daemonProcess = daemonProcess;
        }

        public IDaemonProcess DaemonProcess { get { return _daemonProcess; } }

        public void Execute(Action<DaemonStageResult> committer)
        {
            if (!_daemonProcess.FullRehighlightingRequired)
                return;

            var hightlighings = new List<HighlightingInfo>();

            _daemonProcess.Solution.ForEachProject(p => AnalyzeProject(p, hightlighings));

            committer(new DaemonStageResult(hightlighings));
        }

        private void AnalyzeProject(IProject p, IList<HighlightingInfo> hightlighings)
        {
            // 1. get all types that reside inside namespace
            var types = new List<ICSharpTypeDeclaration>();
            p.ProcessCSharpNodes<ICSharpTypeDeclaration>(type =>
            {
                ICSharpNamespaceDeclaration namespaceDeclaration = type.GetContainingNamespaceDeclaration();

                if (!string.IsNullOrEmpty(type.DeclaredName) && namespaceDeclaration != null)
                    types.Add(type);
            });

            foreach (ICSharpTypeDeclaration type in types)
            {
                string namespaceName = GetShortNamespaceName(type);
                if (string.IsNullOrEmpty(namespaceName) || string.IsNullOrEmpty(type.DeclaredName))
                    continue;

                if (type.DeclaredName.StartsWith(namespaceName))
                {
                    string rightName = type.DeclaredName.Remove(0, namespaceName.Length);
                    if (string.IsNullOrEmpty(rightName))
                        continue;

                    string fullRightName = GetFullNamspaceName(type) + "." + rightName;

                    bool isRightNameExists = types.Any(t =>
                    {
                        if (t.DeclaredElement == null)
                            return false;

                        return t.CLRName.Equals(fullRightName);
                    });

                    if (!isRightNameExists)
                    {
                        hightlighings.Add(new HighlightingInfo(type.NameIdentifier.GetDocumentRange(), new AvoidRedundancyInTypeNameHighlighting(type, rightName)));
                    }
                }
            }
        }

//        private IsTypeExist

        private string GetFullNamspaceName(ICSharpTypeDeclaration type)
        {
            if (type == null)
                return null;

            ICSharpNamespaceDeclaration namespaceDeclaration = type.GetContainingNamespaceDeclaration();
            if (namespaceDeclaration == null)
                return null;

            return namespaceDeclaration.QualifiedName;
        }

        private string GetShortNamespaceName(ICSharpTypeDeclaration type)
        {
            if (type == null)
                return null;

            ICSharpNamespaceDeclaration namespaceDeclaration = type.GetContainingNamespaceDeclaration();
            if (namespaceDeclaration == null)
                return null;

            return namespaceDeclaration.ShortName;
        }
    }
}