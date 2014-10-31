using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.AttributeArgumentsShouldHaveAccessors
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class AttributeArgumentsShouldHaveAccessorsighlighting : IHighlighting
    {
        public IClassDeclaration Declaration { get; private set; }

        public AttributeArgumentsShouldHaveAccessorsighlighting(IClassDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Design: attribute arguments should have accessors."; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}