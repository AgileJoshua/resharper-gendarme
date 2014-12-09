using JetBrains.Application.Settings;
using JetBrains.ReSharper.Settings;

namespace RGendarme.Settings.Naming
{
    [SettingsKey(typeof(CodeInspectionSettings), "Naming rules")]
    public class NamingRulesSettings
    {
        [SettingsEntry(true, "The depth of the namespace hierarchy is getting out of control.")]
        public bool AvoidDeepNamespaceHierarchyEnabled { get; set; }

        [SettingsEntry(true, "This namespace, type or member name contains underscore(s).")]
        public bool AvoidNonAlphanumericIdentifierEnabled { get; set; }

        [SettingsEntry(true, "This method's name includes the type name of the first parameter. This usually makes an API more verbose and less future-proof than necessary.")]
        public bool AvoidRedundancyInMethodNameEnabled { get; set; }

        [SettingsEntry(true, "This type name is prefixed with the last component of its enclosing namespace. This usually makes an API more verbose and less autocompletion-friendly than necessary.")]
        public bool AvoidRedundancyInTypeNameEnabled { get; set; }

        [SettingsEntry(true, "This interface is not implemented by the type of the same name (minus the 'I' prefix).")]
        public bool AvoidTypeInterfaceInconsistencyEnabled { get; set; }

        [SettingsEntry(true, "This type contains event(s) whose names start with either After or Before.")]
        public bool DoNotPrefixEventsWithAfterOrBeforeEnabled { get; set; }

        [SettingsEntry(true, "This enumeration contains value names that start with the enum's name.")]
        public bool DoNotPrefixValuesWithEnumNameEnabled { get; set; }

        [SettingsEntry(true, "This type is an enumeration that contains value(s) named 'reserved'.")]
        public bool DoNotUseReservedInEnumValueNamesEnabled { get; set; }

        [SettingsEntry(true, "This method overrides (or implements) an existing method but does not use the same parameter names as the original.")]
        public bool ParameterNamesShouldMatchOverriddenMethodEnabled { get; set; }

        [SettingsEntry(true, "This type starts with an incorrect prefix or does not start with the required one. All interface names should start with the 'I' letter, followed by another capital letter. All other type names should not have any specific prefix.")]
        public bool UseCorrectPrefixEnabled { get; set; }

        [SettingsEntry(true, "This type does not end with the correct suffix. That usually happens when you define a custom attribute or exception and forget to append suffixes like 'Attribute' or 'Exception' to the type name.")]
        public bool UseCorrectSuffixEnabled { get; set; }

        [SettingsEntry(true, "This type is a flags enumeration and, by convention, should have a plural name.")]
        public bool UsePluralNameInEnumFlagsEnabled { get; set; }

        [SettingsEntry(true, "The identifier contains non-recommended term(s).")]
        public bool UsePreferredTermsEnabled { get; set; }

        [SettingsEntry(true, "This type is an enumeration and by convention it should have a singular name.")]
        public bool UseSingularNameInEnumsUnlessAreFlagsEnabled { get; set; }
    }
}