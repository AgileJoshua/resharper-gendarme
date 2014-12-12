using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.PowerToys.OptionsPage;
using JetBrains.UI.Controls;
using JetBrains.UI.CrossFramework;
using JetBrains.UI.Options;
using RGendarme.Settings.Icons;

namespace RGendarme.Settings.Naming
{
    /// <summary>
    /// Interaction logic for NamingOptionPage.xaml
    /// </summary>
    [OptionsPage(PID, "Naming rules", typeof(OptionsPageThemedIcons.UseIntMaxValueOptions), ParentId = MainOptionPage.PID)]
    public partial class NamingOptionPage : IOptionsPage
    {
        private const string PID = "NamingOptionPage";

        public NamingOptionPage(Lifetime lifetime, OptionsSettingsSmartContext settings)
        {
            InitializeComponent();
            
            settings.SetBinding(lifetime, (NamingRulesSettings s) => s.AvoidDeepNamespaceHierarchyEnabled, cbAvoidDeepNamespaceHierarchy, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (NamingRulesSettings s) => s.AvoidNonAlphanumericIdentifierEnabled, cbAvoidNonAlphanumericIdentifier, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (NamingRulesSettings s) => s.AvoidRedundancyInMethodNameEnabled, cbAvoidRedundancyInMethodName, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (NamingRulesSettings s) => s.AvoidRedundancyInTypeNameEnabled, cbAvoidRedundancyInTypeName, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (NamingRulesSettings s) => s.AvoidTypeInterfaceInconsistencyEnabled, cbAvoidTypeInterfaceInconsistency, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (NamingRulesSettings s) => s.DoNotPrefixEventsWithAfterOrBeforeEnabled, cbDoNotPrefixEventsWithAfterOrBefore, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (NamingRulesSettings s) => s.DoNotPrefixValuesWithEnumNameEnabled, cbDoNotPrefixValuesWithEnumName, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (NamingRulesSettings s) => s.DoNotUseReservedInEnumValueNamesEnabled, cbDoNotUseReservedInEnumValueNames, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (NamingRulesSettings s) => s.ParameterNamesShouldMatchOverriddenMethodEnabled, cbParameterNamesShouldMatchOverriddenMethod, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (NamingRulesSettings s) => s.UseCorrectPrefixEnabled, cbUseCorrectPrefix, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (NamingRulesSettings s) => s.UseCorrectSuffixEnabled, cbUseCorrectSuffix, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (NamingRulesSettings s) => s.UsePluralNameInEnumFlagsEnabled, cbUsePluralNameInEnumFlags, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (NamingRulesSettings s) => s.UsePreferredTermsEnabled, cbUsePreferredTerms, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (NamingRulesSettings s) => s.UseSingularNameInEnumsUnlessAreFlagsEnabled, cbUseSingularNameInEnumsUnlessAreFlags, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);
        }

        public bool OnOk()
        {
            return true;
        }

        public bool ValidatePage()
        {
            return true;
        }

        public EitherControl Control { get { return this; } }
        public string Id { get { return PID; } }
    }
}
