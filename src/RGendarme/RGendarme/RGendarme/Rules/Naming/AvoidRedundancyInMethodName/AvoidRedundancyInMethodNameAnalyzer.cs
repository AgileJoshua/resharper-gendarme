using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Naming.AvoidRedundancyInMethodName
{
    [ElementProblemAnalyzer(new[] { typeof(IMethodDeclaration) }, HighlightingTypes = new[] { typeof(AvoidRedundancyInMethodNameHighlighting) })]
    public class AvoidRedundancyInMethodNameAnalyzer : ElementProblemAnalyzer<IMethodDeclaration>
    {
        protected override void Run(IMethodDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (string.IsNullOrEmpty(element.DeclaredName) || element.ParameterDeclarations.IsEmpty || element.NameIdentifier == null)
                return;

            ICSharpParameterDeclaration first = element.ParameterDeclarations.FirstOrDefault();
            if (first == null || first.TypeUsage == null)
                return;

            string typeName = first.TypeUsage.GetText();
            if (element.DeclaredName.Contains(typeName))
            {
                string errorMsg = string.Format("Use '{0}' instead.", element.DeclaredName.Replace(typeName, string.Empty));
                consumer.AddHighlighting(new AvoidRedundancyInMethodNameHighlighting(element, errorMsg), element.NameIdentifier.GetDocumentRange(), element.NameIdentifier.GetContainingFile());
            }
        }
    }
}