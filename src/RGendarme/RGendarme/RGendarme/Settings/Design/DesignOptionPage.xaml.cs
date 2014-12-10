using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.PowerToys.OptionsPage;
using JetBrains.UI.Controls;
using JetBrains.UI.CrossFramework;
using JetBrains.UI.Options;

namespace RGendarme.Settings.Design
{
    /// <summary>
    /// Interaction logic for DesignOptionPage.xaml
    /// </summary>
    [OptionsPage(PID, "Design rules", typeof(OptionsPageThemedIcons.UseIntMaxValueOptions), ParentId = MainOptionPage.PID)]
    public partial class DesignOptionPage : IOptionsPage
    {
        private const string PID = "DesignOptionPage";

        public DesignOptionPage(Lifetime lifetime, OptionsSettingsSmartContext settings)
        {
            InitializeComponent();

            // TODO: add bingings later

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.AttributeArgumentsShouldHaveAccessorsEnabled, cbAttributeArgumentsShouldHaveAccessors, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.AvoidEmptyInterfaceEnabled, cbAvoidEmptyInterface, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.AvoidMultidimensionalIndexerEnabled, cbAvoidMultidimensionalIndexer, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.AvoidPropertiesWithoutGetAccessorEnabled, cbAvoidPropertiesWithoutGetAccessor, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.AvoidRefAndOutParametersEnabled, cbAvoidRefAndOutParameters, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.AvoidVisibleFieldsEnabled, cbAvoidVisibleFields, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.AvoidVisibleNestedTypesEnabled, cbAvoidVisibleNestedTypes, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.ConsiderAddingInterfaceEnabled, cbConsiderAddingInterface, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.ConsiderConvertingFieldToNullableEnabled, cbConsiderConvertingFieldToNullable, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.ConsiderConvertingMethodToPropertyEnabled, cbConsiderConvertingMethodToProperty, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.ConsiderUsingStaticTypeEnabled, cbConsiderUsingStaticType, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.DeclareEventHandlersCorrectlyEnabled, cbDeclareEventHandlersCorrectly, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.DisposableTypesShouldHaveFinalizerEnabled, cbDisposableTypesShouldHaveFinalizer, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.EnsureSymmetryForOverloadedOperatorsEnabled, cbEnsureSymmetryForOverloadedOperators, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.EnumsShouldDefineAZeroValueEnabled, cbEnumsShouldDefineAZeroValue, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.EnumsShouldUseInt32Enabled, cbEnumsShouldUseInt32, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.FlagsShouldNotDefineAZeroValueEnabled, cbFlagsShouldNotDefineAZeroValue, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.ImplementEqualsAndGetHashCodeInPairEnabled, cbImplementEqualsAndGetHashCodeInPair, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.ImplementICloneableCorrectlyEnabled, cbImplementICloneableCorrectly, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.InternalNamespacesShouldNotExposeTypesEnabled, cbInternalNamespacesShouldNotExposeTypes, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.MainShouldNotBePublicEnabled, cbMainShouldNotBePublic, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.MarkAssemblyWithComVisibleEnabled, cbMarkAssemblyWithComVisible, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.MarkAssemblyWithCLSCompliantEnabled, cbMarkAssemblyWithCLSCompliant, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.MarkAssemblyWithAssemblyVersionEnabled, cbMarkAssemblyWithAssemblyVersion, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.MissingAttributeUsageOnCustomAttributeEnabled, cbMissingAttributeUsageOnCustomAttribute, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.OperatorEqualsShouldBeOverloadedEnabled, cbOperatorEqualsShouldBeOverloaded, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.OverrideEqualsMethodEnabled, cbOverrideEqualsMethod, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.PreferEventsOverMethodsEnabled, cbPreferEventsOverMethods, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.PreferIntegerOrStringForIndexersEnabled, cbPreferIntegerOrStringForIndexers, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.PreferXmlAbstractionsEnabled, cbPreferXmlAbstractions, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.ProvideAlternativeNamesForOperatorOverloadsEnabled, cbProvideAlternativeNamesForOperatorOverloads, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.TypesShouldBeInsideNamespacesEnabled, cbTypesShouldBeInsideNamespaces, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.TypesWithDisposableFieldsShouldBeDisposableEnabled, cbTypesWithDisposableFieldsShouldBeDisposable, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.TypesWithNativeFieldsShouldBeDisposableEnabled, cbTypesWithNativeFieldsShouldBeDisposable, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.UseCorrectDisposeSignaturesEnabled, cbUseCorrectDisposeSignatures, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

            settings.SetBinding(lifetime, (DesignRulesSettings s) => s.UseFlagsAttributeEnabled, cbUseFlagsAttribute, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);
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
