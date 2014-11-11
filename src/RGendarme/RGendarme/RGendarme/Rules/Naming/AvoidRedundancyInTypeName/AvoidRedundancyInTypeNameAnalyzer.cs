using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Naming.AvoidRedundancyInTypeName
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(AvoidRedundancyInTypeNameHighlighting) })]
    public class AvoidRedundancyInTypeNameAnalyzer : ElementProblemAnalyzer<IClassDeclaration>
    {
        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            INamespaceDeclaration namespaceDeclaration = element.GetContainingNamespaceDeclaration();
            if (namespaceDeclaration == null || namespaceDeclaration.DeclaredElement == null || element.NameIdentifier == null)
                return;

            // *************************************
            //INamespace n1 = namespaceDeclaration.DeclaredElement;
            // *************************************

            string name = namespaceDeclaration.DeclaredElement.ShortName;

            if (!string.IsNullOrEmpty(element.DeclaredName) && element.DeclaredName.StartsWith(name))
            {
                string errorMsg = string.Format("Use '{0}' instead.", element.DeclaredName.Replace(name, string.Empty));

                consumer.AddHighlighting(new AvoidRedundancyInTypeNameHighlighting(element, errorMsg), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }
        }
    }

    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class AvoidRedundancyInTypeNameHighlighting : IHighlighting
    {
        public IClassDeclaration Declaration { get; private set; }

        private readonly string _errorMessage;

        public AvoidRedundancyInTypeNameHighlighting(IClassDeclaration declaration, string errorMessage)
        {
            Declaration = declaration;
            _errorMessage = errorMessage;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Naming: avoid redundancy in type name. " + _errorMessage; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}