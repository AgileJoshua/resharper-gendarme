using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.OperatorEqualsShouldBeOverloaded
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class OperatorEqualsShouldBeOverloadedHighlighting : IHighlighting
    {
        public OperatorEqualsShouldBeOverloadedHighlighting(ICSharpTypeDeclaration declaration)
        {
            Declaration = declaration;
        }

        public ICSharpTypeDeclaration Declaration { get; private set; }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Design: It should also implement the equality (==) operator."; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}