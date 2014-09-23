using System;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Tutorial.IntPower
{
    [ElementProblemAnalyzer(new[] { typeof(IInvocationExpression) }, HighlightingTypes = new[] { typeof(IntPowerHighlighting) })]
    public class IntPowerProblemAnalyzer : ElementProblemAnalyzer<IInvocationExpression>
    {   
        protected override void Run(IInvocationExpression element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            bool functionIsCalledPow = false;
            var e = element.InvokedExpression as IReferenceExpression;
            if (e != null)
            {
                if (e.Reference.GetName().Equals("Pow"))
                    functionIsCalledPow = true;
            }

            bool firstArgIsIdentifier = false;
            bool secondArgIsInteger = false;
            int power = -1;
            if (element.Arguments.Count == 2)
            {
                firstArgIsIdentifier = element.Arguments[0].Value is IReferenceExpression;

                var secondArg = element.Arguments[1].Value as ICSharpLiteralExpression;
                if (secondArg != null && secondArg.IsConstantValue())
                {
                    double value = -1.0;

                    ConstantValue cv = secondArg.ConstantValue;
                    if (cv.IsDouble())
                        value = (double) cv.Value;
                    else if (cv.IsInteger())
                        value = (int) cv.Value;

                    power = (int) value;

                    secondArgIsInteger = value > 0.0 && value == Math.Floor(value);
                }
            }

            if (functionIsCalledPow && firstArgIsIdentifier && secondArgIsInteger)
                consumer.AddHighlighting(new IntPowerHighlighting(element, power), element.GetDocumentRange(), element.GetContainingFile());
        }
    }
}