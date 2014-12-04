using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib.Extenstions;

namespace RGendarme.Rules.Naming.AvoidNonAlphanumericIdentifier
{
    [ElementProblemAnalyzer(new[] { typeof(ICSharpDeclaration) }, HighlightingTypes = new[] { typeof(AvoidNonAlphanumericIdentifierHighlighting) })]
    public class AvoidNonAlphanumericIdentifierAnalyzer : ElementProblemAnalyzer<ICSharpDeclaration>
    {
        protected override void Run(ICSharpDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var namespaceDecl = element as ICSharpNamespaceDeclaration;
            if (namespaceDecl != null)
            {
                if (!CheckName(namespaceDecl.QualifiedName, false) && namespaceDecl.NameIdentifier != null)
                {
                    consumer.AddHighlighting(new AvoidNonAlphanumericIdentifierHighlighting(namespaceDecl), namespaceDecl.NameIdentifier.GetDocumentRange(), namespaceDecl.GetContainingFile());
                }
            }

            var methodDecl = element as IMethodDeclaration;
            if (methodDecl != null && methodDecl.IsPublic())
            {
                if (!CheckName(methodDecl.DeclaredName, false) && methodDecl.NameIdentifier != null)
                {
                    consumer.AddHighlighting(new AvoidNonAlphanumericIdentifierHighlighting(methodDecl), methodDecl.NameIdentifier.GetDocumentRange(), methodDecl.GetContainingFile());
                }
            }

            // TODO: throw new System.NotImplementedException();
        }

        #region Private methods (from Gendarme)
        // Compiler generates an error for any other non alpha-numerics than underscore ('_'), 
        // so we just need to check the presence of underscore in method names
        private static bool CheckName(string name, bool special)
        {
            int start = special ? name.IndexOf('_') + 1 : 0;
            return (name.IndexOf('_', start) == -1);
        }
        #endregion
    }
}