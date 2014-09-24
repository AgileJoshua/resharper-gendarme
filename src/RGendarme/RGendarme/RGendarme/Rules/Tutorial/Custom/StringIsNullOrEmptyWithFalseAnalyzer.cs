using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Tutorial.Custom
{
    [ElementProblemAnalyzer(new[] { typeof(IEqualityExpression) }, HighlightingTypes = new[] { typeof(StringIsNullOrEmptyWithFalseHighlight) })]
    public class StringIsNullOrEmptyWithFalseAnalyzer : ElementProblemAnalyzer<IEqualityExpression>
    {
        protected override void Run(IEqualityExpression element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            bool isNullOrEmptyFunc = false;
            var left = element.LeftOperand as IInvocationExpression;
            if (left != null)
            {
                var r = left.InvocationExpressionReference.Resolve();
                var method = r.DeclaredElement as IMethod;
                if (method != null)
                {
                    var parent = method.GetContainingType();
                    if (parent != null)
                    {
                        isNullOrEmptyFunc = parent.GetClrName().FullName.Equals("System.String")
                                            && method.ShortName.Equals("IsNullOrEmpty");
                    }
                }
            }

            bool isFalseConstant = false;
            var right = element.RightOperand as ICSharpLiteralExpression;
            if (right != null && right.IsConstantValue())
            {
                ConstantValue cv = right.ConstantValue;
                if (cv.IsBoolean())
                {
                    var value = (bool) cv.Value;
                    isFalseConstant = !value;
                }
            }

            if (isNullOrEmptyFunc && isFalseConstant)
                consumer.AddHighlighting(new StringIsNullOrEmptyWithFalseHighlight(element), element.GetDocumentRange(), element.GetContainingFile());
        }
    }

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