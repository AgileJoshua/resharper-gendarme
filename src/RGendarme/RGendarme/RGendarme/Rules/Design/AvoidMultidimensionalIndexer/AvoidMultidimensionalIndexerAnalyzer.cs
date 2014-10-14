using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Design.AvoidMultidimensionalIndexer
{
    [ElementProblemAnalyzer(new[] { typeof(IIndexerDeclaration) }, HighlightingTypes = new[] { typeof(AvoidMultidimensionalIndexerHighlighting) })]
    public class AvoidMultidimensionalIndexerAnalyzer : ElementProblemAnalyzer<IIndexerDeclaration>
    {
        protected override void Run(IIndexerDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            IFormalParameterList parameters = element.Params;
            if (parameters == null) return;

            if (parameters.ParameterDeclarations.Count > 1)
            {
                var thisKeyword = element.ThisKeyword;

                consumer.AddHighlighting(new AvoidMultidimensionalIndexerHighlighting(element), thisKeyword.GetDocumentRange(), thisKeyword.GetContainingFile());    
            }
        }
    }
}