using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Naming.UsePropertyInsteadMethodWithGetPrefix
{
    [ElementProblemAnalyzer(new[] { typeof(IMethodDeclaration) }, HighlightingTypes = new[] { typeof(UsePropertyInsteadMethodWithGetPrefixHighlighting) })]
    public class UsePropertyInsteadMethodWithGetPrefixAnalyzer : ElementProblemAnalyzer<IMethodDeclaration>
    {
        protected override void Run(IMethodDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            // 1. Method name starts with 'Get'
            if (element.NameIdentifier == null || string.IsNullOrEmpty(element.NameIdentifier.Name)) 
                return;

            if (!element.NameIdentifier.Name.StartsWith("Get"))
                return;

            // 2. parameters list is empty
            if (element.Params == null || element.Params.ParameterDeclarations.IsEmpty)
            {
                consumer.AddHighlighting(new UsePropertyInsteadMethodWithGetPrefixHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }
        }
    }
}