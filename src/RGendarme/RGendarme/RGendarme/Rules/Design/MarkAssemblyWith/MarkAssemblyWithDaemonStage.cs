using System.Collections.Generic;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using RGendarme.Settings.Design;

namespace RGendarme.Rules.Design.MarkAssemblyWith
{
    [DaemonStage(StagesBefore = new[] { typeof(LanguageSpecificDaemonStage) })]
    public class MarkAssemblyWithDaemonStage : IDaemonStage
    {
        public ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settingsStore)
        {
            return ErrorStripeRequest.STRIPE_AND_ERRORS;
        }

        public IEnumerable<IDaemonStageProcess> CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
        {
            var setting = settings.GetKey<DesignRulesSettings>(SettingsOptimization.OptimizeDefault);

            var enabledRules = new MarkAssemblyWithRules
            {
                ComVisibleEnabled = setting.MarkAssemblyWithComVisibleEnabled,
                ClsCompliantEnabled = setting.MarkAssemblyWithCLSCompliantEnabled,
                AssemblyVersion = setting.MarkAssemblyWithAssemblyVersionEnabled
            };

            return new[] {new MarkAssemblyWithDaemonProcess(process, enabledRules)};
        }
    }

    public class MarkAssemblyWithRules
    {
        public bool ComVisibleEnabled { get; set; }

        public bool ClsCompliantEnabled { get; set; }

        public bool AssemblyVersion { get; set; }

        public bool IsAllDisabled
        {
            get { return !ComVisibleEnabled && !ClsCompliantEnabled && !AssemblyVersion; }
        }
    }
}