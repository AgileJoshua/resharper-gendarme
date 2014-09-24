using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Custom.StringIsNullOrEmptyWithFalse
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class StringIsNullOrEmptyWithFalseHighlight : IHighlighting
    {
        public IEqualityExpression Expression { get; private set; }

        public StringIsNullOrEmptyWithFalseHighlight(IEqualityExpression expression)
        {
            Expression = expression;
        }

        public bool IsValid()
        {
            return Expression != null && Expression.IsValid();
        }

        public string ToolTip { get { return "Redundant use of false constant"; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch {get { return 0; } }
    }
}