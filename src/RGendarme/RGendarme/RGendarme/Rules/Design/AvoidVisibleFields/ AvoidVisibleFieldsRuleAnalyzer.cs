using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Design.AvoidVisibleFields
{
    [ElementProblemAnalyzer(new[] { typeof(IMultipleFieldDeclaration) }, HighlightingTypes = new[] { typeof(AvoidVisibleFieldsHighlighting) })]
    public class  AvoidVisibleFieldsAnalyzer : ElementProblemAnalyzer<IMultipleFieldDeclaration>
    {
        protected override void Run(IMultipleFieldDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            bool isPublicField = false;
            IModifiersList modifiers = element.ModifiersList;

            foreach (ITokenNode m in modifiers.Modifiers)
            {
                if (m.GetText().Equals("public"))
                    isPublicField = true;
            }

            if (isPublicField)
            {
                consumer.AddHighlighting(new AvoidVisibleFieldsHighlighting(element), element.GetDocumentRange(), element.GetContainingFile());
            }
        }
    }
}