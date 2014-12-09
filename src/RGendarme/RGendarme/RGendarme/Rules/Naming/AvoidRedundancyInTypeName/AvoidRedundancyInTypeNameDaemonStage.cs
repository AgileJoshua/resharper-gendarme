using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using RGendarme.Lib.Extenstions;
using RGendarme.Settings.Naming;

namespace RGendarme.Rules.Naming.AvoidRedundancyInTypeName
{
    [DaemonStage(StagesBefore = new[] { typeof(LanguageSpecificDaemonStage) })]
    internal class AvoidRedundancyInTypeNameDaemonStage : IDaemonStage
    {
        public IEnumerable<IDaemonStageProcess> CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
        {
            var s = settings.GetKey<NamingRulesSettings>(SettingsOptimization.OptimizeDefault);
            
            if (!s.AvoidRedundancyInTypeNameEnabled || !process.Solution.HasCSharpFile())
                return Enumerable.Empty<IDaemonStageProcess>();

            return new[] {new AvoidRedundancyInTypeNameDaemonProcess(process)};
        }

        public ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settingsStore)
        {
            return ErrorStripeRequest.STRIPE_AND_ERRORS;
        }
    }
}