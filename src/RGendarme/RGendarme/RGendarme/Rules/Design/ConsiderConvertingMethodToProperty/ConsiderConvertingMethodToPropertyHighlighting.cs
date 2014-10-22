using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.ConsiderConvertingMethodToProperty
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class ConsiderConvertingMethodToPropertyHighlighting : IHighlighting
    {
        public IMethodDeclaration Declaration { get; private set; }

        public ConsiderConvertingMethodToPropertyHighlighting(IMethodDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Design: consider converting method to property."; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}