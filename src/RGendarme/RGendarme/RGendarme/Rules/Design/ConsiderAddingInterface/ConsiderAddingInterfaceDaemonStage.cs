using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using RGendarme.Lib.Extenstions;

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
            if (!process.Solution.HasCSharpFile())
                return Enumerable.Empty<IDaemonStageProcess>();

            return new[] {new ConsiderAddingInterfaceDaemonProcess(process)};
        }
    }
}