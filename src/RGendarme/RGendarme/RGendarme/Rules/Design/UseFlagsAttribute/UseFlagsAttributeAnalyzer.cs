using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib;

namespace RGendarme.Rules.Design.UseFlagsAttribute
{
    [ElementProblemAnalyzer(new[] { typeof(IEnumDeclaration) }, HighlightingTypes = new[] { typeof(UseFlagsAttributeHighlighting) })]
    public class UseFlagsAttributeAnalyzer : ElementProblemAnalyzer<IEnumDeclaration>
    {
        protected override void Run(IEnumDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (element.EnumBody == null)
                return;

            // 1. check if enum has bitwise operation
            bool hasBitwiseOperation = false;
            foreach (IEnumMemberDeclaration member in element.EnumMemberDeclarations)
            {
                if (member.ValueExpression is IBitwiseInclusiveOrExpression)
                {
                    hasBitwiseOperation = true;
                    break;
                }
            }

            if (!hasBitwiseOperation)
                return;

            // 2. if has, check for Flags attribute
            if (!AnalyzerHelper.HasAttribute(element, "System.FlagsAttribute"))
            {
                consumer.AddHighlighting(new UseFlagsAttributeHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }
        }
    }
}