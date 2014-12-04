using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib;
using RGendarme.Lib.Extenstions;

namespace RGendarme.Rules.Design.ConsiderAddingInterface
{
    internal class ConsiderAddingInterfaceDaemonProcess : IDaemonStageProcess
    {
        private readonly IDaemonProcess _daemonProcess;

        public ConsiderAddingInterfaceDaemonProcess([NotNull] IDaemonProcess daemonProcess)
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
            var interaces = new List<IInterfaceDeclaration>();
            p.ProcessCSharpNodes<IInterfaceDeclaration>(interaces.Add);

            // todo fefactor it - use lambda instead of foreach loop
            var classes = new List<IClassDeclaration>();
            p.ProcessCSharpNodes<IClassDeclaration>(classes.Add);

            foreach (IClassDeclaration cls in classes)
            {
                if (cls.MethodDeclarations.IsEmpty)
                    continue;

                foreach (IInterfaceDeclaration inter in interaces)
                {
                    if (inter.MethodDeclarations.IsEmpty)
                        continue;

                    // 1. if class does not implement interface
                    if (AnalyzerHelper.IsImplement(cls, inter))
                        continue;

                    // 2. but have all methods from interface
                    bool isFullyImplemented = true;
                    foreach (IMethodDeclaration method in inter.MethodDeclarationsEnumerable)
                    {
                        if (!cls.MethodDeclarationsEnumerable.Any(m => m.ModifiersList != null
                                && m.ModifiersList.HasModifier(CSharpTokenType.PUBLIC_KEYWORD)
                                && !m.ModifiersList.HasModifier(CSharpTokenType.STATIC_KEYWORD)
                                && m.HasSameSignatureAs(method)))
                        {
                            isFullyImplemented = false;
                            break;
                        }
                    }

                    // 3. warning about this
                    if (isFullyImplemented)
                    {
                        hightlighings.Add(new HighlightingInfo(cls.NameIdentifier.GetDocumentRange(), new ConsiderAddingInterfaceHighlighting(cls, inter.NameIdentifier.Name)));
                    }
                }
            }
        }
    }
}