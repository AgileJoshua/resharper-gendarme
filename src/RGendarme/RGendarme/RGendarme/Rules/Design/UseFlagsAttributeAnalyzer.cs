using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Design
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
            var attributes = element.Attributes;
            if (attributes.Count == 0)
                return;

            bool hasFlagsAttribute = false;
            foreach (IAttribute attr in attributes)
            {
                IReferenceName name = attr.Name;
                if (name == null) continue;

                var result = name.Reference.CurrentResolveResult;
                if (result == null) continue;

                var cls = result.DeclaredElement as IClass;
                if (cls == null) continue;

                var clrName = cls.GetClrName().FullName;
                if (clrName.Equals("System.FlagsAttribute"))
                {
                    hasFlagsAttribute = true;
                    break;
                }
            }

            if (!hasFlagsAttribute)
            {
                consumer.AddHighlighting(new UseFlagsAttributeHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }
        }
    }
}