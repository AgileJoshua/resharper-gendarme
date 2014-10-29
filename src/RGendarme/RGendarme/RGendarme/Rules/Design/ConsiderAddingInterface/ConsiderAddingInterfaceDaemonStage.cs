using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Design.ConsiderAddingInterface
{
    [DaemonStage(StagesBefore = new []{typeof(LanguageSpecificDaemonStage)})]
    internal class ConsiderAddingInterfaceDaemonStage : IDaemonStage
    {
        public ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settingsStore)
        {
            return ErrorStripeRequest.STRIPE_AND_ERRORS;
        }

        public IEnumerable<IDaemonStageProcess> CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
        {
            IPsiSourceFile sourceFile = process.SourceFile; // try later with solution
            IPsiServices psiServices = sourceFile.GetPsiServices();

            IFile psiFile = psiServices.Files.GetDominantPsiFile<CSharpLanguage>(sourceFile);
            if (psiFile == null)
                return Enumerable.Empty<IDaemonStageProcess>();

            return new[] {new ConsiderAddingInterfaceDaemonProcess(process)};
        }
    }

    internal class ConsiderAddingInterfaceDaemonProcess : IDaemonStageProcess
    {
        private readonly IDaemonProcess _daemonProcess;

        public ConsiderAddingInterfaceDaemonProcess(IDaemonProcess daemonProcess)
        {
            _daemonProcess = daemonProcess;
        }

        public IDaemonProcess DaemonProcess { get { return _daemonProcess; } }

        public void Execute(Action<DaemonStageResult> committer)
        {
            if (!_daemonProcess.FullRehighlightingRequired)
                return;

            var hightlighings = new List<HighlightingInfo>();

            IPsiSourceFile sourceFile = _daemonProcess.SourceFile;
            IPsiServices psiServices = sourceFile.GetPsiServices();

            IFile file = psiServices.Files.GetDominantPsiFile<CSharpLanguage>(sourceFile);
            if (file == null)
                return;

            file.ProcessChildren<IInterfaceDeclaration>(expression => Analyze(expression, hightlighings));

            committer(new DaemonStageResult(hightlighings));
        }

        private void Analyze(IInterfaceDeclaration element, IList<HighlightingInfo> highlightings)
        {
            if (element.NameIdentifier == null || string.IsNullOrEmpty(element.NameIdentifier.Name))
                return;

            string name = element.NameIdentifier.Name;
            if (name.StartsWith("Test"))
            {
                highlightings.Add(new HighlightingInfo(element.NameIdentifier.GetDocumentRange(), new ConsiderAddingInterfaceHighlighting(element)));
            }
        }
    }

    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class ConsiderAddingInterfaceHighlighting : IHighlighting
    {
        public IInterfaceDeclaration Declaration { get; private set; }

        public ConsiderAddingInterfaceHighlighting(IInterfaceDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Design: consider adding interface."; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}