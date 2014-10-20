using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Naming.UseCorrectPrefix
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class UseCorrectPrefixWrongGenericTypeNameHighlighting : IHighlighting
    {
        public ITypeParameterOfTypeDeclaration Declaration { get; private set; }

        public UseCorrectPrefixWrongGenericTypeNameHighlighting(ITypeParameterOfTypeDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Naming: generic type param name must starts with T letter."; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}