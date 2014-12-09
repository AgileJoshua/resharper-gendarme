using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using RGendarme.Lib.Extenstions;
using RGendarme.Settings.Naming;

namespace RGendarme.Rules.Naming.AvoidTypeInterfaceInconsistency
{
    [DaemonStage(StagesBefore = new[] { typeof(LanguageSpecificDaemonStage) })]
    public class AvoidTypeInterfaceInconsistencyDeamonStage : IDaemonStage
    {
        public ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settingsStore)
        {
            return ErrorStripeRequest.STRIPE_AND_ERRORS;
        }

        public IEnumerable<IDaemonStageProcess> CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
        {
            var s = settings.GetKey<NamingRulesSettings>(SettingsOptimization.OptimizeDefault);

            if (!s.AvoidTypeInterfaceInconsistencyEnabled || !process.Solution.HasCSharpFile())
                return Enumerable.Empty<IDaemonStageProcess>();

            return new[] {new AvoidTypeInterfaceInconsistencyDaemonProcess(process) };
        }
    }
}