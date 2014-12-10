using JetBrains.Application.Settings;
using JetBrains.ReSharper.Settings;

namespace RGendarme.Settings.Design
{
    [SettingsKey(typeof(CodeInspectionSettings), "Design rules")]
    public class DesignRulesSettings
    {
        [SettingsEntry(true, "All parameter values passed to this type's constructors should be visible through read-only properties.")]
        public bool AttributeArgumentsShouldHaveAccessorsEnabled { get; set; }

        [SettingsEntry(true, "This interface does not define any members. This is often a sign that the interface is used simply to mark up types.")]
        public bool AvoidEmptyInterfaceEnabled { get; set; }

        [SettingsEntry(true, "This indexer use multiple indexes which can impair its usability.")]
        public bool AvoidMultidimensionalIndexerEnabled { get; set; }

        [SettingsEntry(true, "This type contains properties which only have setters.")]
        public bool AvoidPropertiesWithoutGetAccessorEnabled { get; set; }

        [SettingsEntry(true, "This method use ref and/or out parameters in a visible API which can confuse many developers.")]
        public bool AvoidRefAndOutParametersEnabled { get; set; }

#warning set settings title
        [SettingsEntry(true, "Avoid visible fields")]
        public bool AvoidVisibleFieldsEnabled { get; set; }

        [SettingsEntry(true, "This type is both nested and visible outside the assembly. Nested types are often confused with namespaces.")]
        public bool AvoidVisibleNestedTypesEnabled { get; set; }

        [SettingsEntry(true, "This type implements an interface's members, but does not implement the interface.")]
        public bool ConsiderAddingInterfaceEnabled { get; set; }

        [SettingsEntry(true, "This field looks like it can be simplified using a nullable type.")]
        public bool ConsiderConvertingFieldToNullableEnabled { get; set; }

        [SettingsEntry(true, "This method looks like a candidate to be a property.")]
        public bool ConsiderConvertingMethodToPropertyEnabled { get; set; }

        [SettingsEntry(true, "This type contains only static fields and methods and should be static.")]
        public bool ConsiderUsingStaticTypeEnabled { get; set; }

        [SettingsEntry(true, "The event has an incorrect signature.")]
        public bool DeclareEventHandlersCorrectlyEnabled { get; set; }

        [SettingsEntry(true, "This type contains native fields but does not have a finalizer.")]
        public bool DisposableTypesShouldHaveFinalizerEnabled { get; set; }

        [SettingsEntry(true, "This type should overload symetric operators together (e.g. == and !=, + and -).")]
        public bool EnsureSymmetryForOverloadedOperatorsEnabled { get; set; }

        [SettingsEntry(true, "This enumeration does not provide a member with a value of 0.")]
        public bool EnumsShouldDefineAZeroValueEnabled { get; set; }

        [SettingsEntry(true, "Unless required for interoperability this enumeration should use Int32 as its underling storage type.")]
        public bool EnumsShouldUseInt32Enabled { get; set; }

        [SettingsEntry(true, "This enumeration flag defines a value of 0, which cannot be used in boolean operations.")]
        public bool FlagsShouldNotDefineAZeroValueEnabled { get; set; }

        [SettingsEntry(true, "This type only implements one of Equals(Object) and GetHashCode().")]
        public bool ImplementEqualsAndGetHashCodeInPairEnabled { get; set; }

#warning set settings title
        [SettingsEntry(true, "Implement ICloneable correctly")]
        public bool ImplementICloneableCorrectlyEnabled { get; set; }

        [SettingsEntry(true, "This internal namespace should not expose visible types outside the assembly.")]
        public bool InternalNamespacesShouldNotExposeTypesEnabled { get; set; }

        [SettingsEntry(true, "The entry point (Main) of this assembly is visible to the outside world (ref: C# Programming Guide).")]
        public bool MainShouldNotBePublicEnabled { get; set; }

        [SettingsEntry(true, "This assembly is not decorated with the [ComVisible] attribute.")]
        public bool MarkAssemblyWithComVisibleEnabled { get; set; }

        [SettingsEntry(true, "This assembly is not decorated with the [CLSCompliant] attribute.")]
        public bool MarkAssemblyWithCLSCompliantEnabled { get; set; }

        [SettingsEntry(true, "This assembly is not decorated with the [AssemblyVersion] attribute.")]
        public bool MarkAssemblyWithAssemblyVersionEnabled { get; set; }

        [SettingsEntry(true, "This attribute does not specify the items it can be used upon.")]
        public bool MissingAttributeUsageOnCustomAttributeEnabled { get; set; }

        [SettingsEntry(true, "This type is a value type and overrides the Equals method or it overloads + and - operators but does not overload the == operator.")]
        public bool OperatorEqualsShouldBeOverloadedEnabled { get; set; }

        [SettingsEntry(true, "This type overloads the == operator but doesn't override the Equals method.")]
        public bool OverrideEqualsMethodEnabled { get; set; }

        [SettingsEntry(true, "This method's name suggests that it could be replaced by an event.")]
        public bool PreferEventsOverMethodsEnabled { get; set; }

        [SettingsEntry(true, "This indexer should be using integers or strings for its indexes.")]
        public bool PreferIntegerOrStringForIndexersEnabled { get; set; }

        [SettingsEntry(true, "This visible method uses XmlDocument, XPathDocument or XmlNode in its signature. This makes changing the implementation more difficult than it should be.")]
        public bool PreferXmlAbstractionsEnabled { get; set; }

        [SettingsEntry(true, "This type contains overloaded operators but doesn't provide named alternatives.")]
        public bool ProvideAlternativeNamesForOperatorOverloadsEnabled { get; set; }

        [SettingsEntry(true, "This type wis visible outside the assembly so it should be defined inside a namespace to avoid conflicts.")]
        public bool TypesShouldBeInsideNamespacesEnabled { get; set; }

#warning set settings title
        [SettingsEntry(true, "Types with disposable fields should be disposable")]
        public bool TypesWithDisposableFieldsShouldBeDisposableEnabled { get; set; }

#warning set settings title
        [SettingsEntry(true, "Types with native fields should be disposable")]
        public bool TypesWithNativeFieldsShouldBeDisposableEnabled { get; set; }

        [SettingsEntry(true, "An IDisposable type does not conform to the guidelines for its Dispose methods.")]
        public bool UseCorrectDisposeSignaturesEnabled { get; set; }

        [SettingsEntry(true, "The enum seems to be composed of flag values, but is not decorated with [Flags].")]
        public bool UseFlagsAttributeEnabled { get; set; }
    }
}