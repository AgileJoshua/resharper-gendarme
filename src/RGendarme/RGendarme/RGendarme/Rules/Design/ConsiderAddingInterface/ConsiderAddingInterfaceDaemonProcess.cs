using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
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

            DaemonProcess.SourceFile.EnumerateDominantPsiFiles();

            var hightlighings = new List<HighlightingInfo>();

            _daemonProcess.Solution.ProccessCSharpFiles<IInterfaceDeclaration>(expression => Analyze(expression, hightlighings));

            committer(new DaemonStageResult(hightlighings));
        }

        private void Analyze(IInterfaceDeclaration element, IList<HighlightingInfo> highlightings)
        {
            if (element.NameIdentifier == null || string.IsNullOrEmpty(element.NameIdentifier.Name))
                return;

            string name = element.NameIdentifier.Name;
            if (name.StartsWith("Test"))
            {
                highlightings.Add(new HighlightingInfo(element.NameIdentifier.GetDocumentRange(), new ConsiderAddingInterfaceHighlighting(element, "bla-bla-bla")));
            }
        }
    }
}