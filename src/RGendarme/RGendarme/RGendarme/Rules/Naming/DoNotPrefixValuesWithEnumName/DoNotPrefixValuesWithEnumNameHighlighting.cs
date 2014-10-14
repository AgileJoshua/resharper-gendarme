using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Naming.DoNotPrefixValuesWithEnumName
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class DoNotPrefixValuesWithEnumNameHighlighting : IHighlighting
    {
        public IEnumMemberDeclaration Declaration { get; private set; }
        public DoNotPrefixValuesWithEnumNameHighlighting(IEnumMemberDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Do not prefix values with enum name."; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}