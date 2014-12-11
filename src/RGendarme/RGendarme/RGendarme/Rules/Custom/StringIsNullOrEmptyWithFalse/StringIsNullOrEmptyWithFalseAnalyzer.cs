using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Custom.StringIsNullOrEmptyWithFalse
{
    [ElementProblemAnalyzer(new[] { typeof(IEqualityExpression) }, HighlightingTypes = new[] { typeof(StringIsNullOrEmptyWithFalseHighlight) })]
    public class StringIsNullOrEmptyWithFalseAnalyzer : ElementProblemAnalyzer<IEqualityExpression>
    {
        private readonly ISettingsStore _settings;
        public StringIsNullOrEmptyWithFalseAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

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
                    ITypeElement parent = method.GetContainingType();
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
}