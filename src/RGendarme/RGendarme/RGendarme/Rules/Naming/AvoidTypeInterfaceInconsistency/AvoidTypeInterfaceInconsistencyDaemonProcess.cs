using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib.Extenstions;

namespace RGendarme.Rules.Naming.AvoidTypeInterfaceInconsistency
{
    internal class AvoidTypeInterfaceInconsistencyDaemonProcess : IDaemonStageProcess
    {
        private readonly IDaemonProcess _daemonProcess;

        public AvoidTypeInterfaceInconsistencyDaemonProcess([NotNull]IDaemonProcess daemonProcess)
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

        private void AnalyzeProject(IProject p, IList<HighlightingInfo> highlightings)
        {
            var interaces = new List<IInterfaceDeclaration>();
            p.ProcessCSharpNodes<IInterfaceDeclaration>(interaces.Add);

            p.ProcessCSharpNodes<IClassDeclaration>(c => AnalyzeClass(c, interaces, highlightings));
        }

        private void AnalyzeClass(IClassDeclaration classDeclaration, IList<IInterfaceDeclaration> interfaces, IList<HighlightingInfo> highlightings)
        {
            if (classDeclaration.NameIdentifier == null)
                return;

            var interfaceName = "I" + classDeclaration.NameIdentifier.Name;
            IInterfaceDeclaration existedInterface =
                interfaces.FirstOrDefault(
                    i => !string.IsNullOrEmpty(i.DeclaredName) && i.DeclaredName.Equals(interfaceName));

            if (existedInterface != null)
            {
                highlightings.Add(new HighlightingInfo(classDeclaration.NameIdentifier.GetDocumentRange(), new AvoidTypeInterfaceInconsistencyHighlighting(classDeclaration, existedInterface)));
            }
        }
    }
}