using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Tutorial.IntPower
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class IntPowerHighlighting : IHighlighting
    {
        public IInvocationExpression Expression { get; private set; }
        public int Power { get; private set; }

        public IntPowerHighlighting(IInvocationExpression expression, int power)
        {
            Expression = expression;
            Power = power;
        }

        public bool IsValid()
        {
            return Expression != null && Expression.IsValid();
        }

        public string ToolTip { get { return "Inefficient use of integer-based power"; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}