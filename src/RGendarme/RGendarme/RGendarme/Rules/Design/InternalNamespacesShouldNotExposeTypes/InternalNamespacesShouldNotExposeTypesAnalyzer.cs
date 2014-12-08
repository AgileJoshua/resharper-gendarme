using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib.Extenstions;

namespace RGendarme.Rules.Design.InternalNamespacesShouldNotExposeTypes
{
    [ElementProblemAnalyzer(new[] { typeof(ICSharpTypeDeclaration) }, HighlightingTypes = new[] { typeof(InternalNamespacesShouldNotExposeTypesHighlighting) })]
    public class InternalNamespacesShouldNotExposeTypesAnalyzer : ElementProblemAnalyzer<ICSharpTypeDeclaration>
    {
        private readonly ISettingsStore _settings;

        public InternalNamespacesShouldNotExposeTypesAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(ICSharpTypeDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            // 1. rule works only with visible types
            if (!element.IsPublic() || element.NameIdentifier == null)
                return;

            ICSharpNamespaceDeclaration namespaceDecl = element.GetContainingNamespaceDeclaration();
            if (namespaceDecl == null)
                return;

            string name = namespaceDecl.ShortName;

            if (!string.IsNullOrEmpty(name) && (name.Equals("Internal") /*|| name.Equals("Impl") */))
            {
                consumer.AddHighlighting(new InternalNamespacesShouldNotExposeTypesHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }
        }
    }
}