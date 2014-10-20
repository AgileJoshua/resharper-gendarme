using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.AvoidPropertiesWithoutGetAccessor
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class AvoidPropertiesWithoutGetAccessorHighlighting : IHighlighting
    {
        public IPropertyDeclaration Declaration { get; private set; }

        public AvoidPropertiesWithoutGetAccessorHighlighting(IPropertyDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Design: avoid properties without get accessor."; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}