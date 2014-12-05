using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Design.TypesShouldBeInsideNamespaces
{
    [ElementProblemAnalyzer(new[] { typeof(ICSharpTypeDeclaration) }, HighlightingTypes = new[] { typeof(TypesShouldBeInsideNamespacesHighlighting) })]
    public class TypesShouldBeInsideNamespacesAnalyzer : ElementProblemAnalyzer<ICSharpTypeDeclaration>
    {
        private readonly ISettingsStore _settings;

        public TypesShouldBeInsideNamespacesAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(ICSharpTypeDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            ICSharpNamespaceDeclaration namespaceDecl = element.GetContainingNamespaceDeclaration();
            if (namespaceDecl == null && element.NameIdentifier != null)
            {
                consumer.AddHighlighting(new TypesShouldBeInsideNamespacesHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }
        }
    }
}