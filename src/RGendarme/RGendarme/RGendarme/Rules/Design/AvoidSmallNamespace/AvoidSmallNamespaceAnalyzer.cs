using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.AvoidSmallNamespace
{
#if false
    [ElementProblemAnalyzer(new[] { typeof(ICSharpNamespaceDeclaration) }, HighlightingTypes = new[] { typeof(AvoidSmallNamespaceHighlighting) })]
    public class AvoidSmallNamespaceAnalyzer : ElementProblemAnalyzer<ICSharpNamespaceDeclaration>
    {
        protected override void Run(ICSharpNamespaceDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (element.Body == null)
                return;

            INamespaceBody body = element.Body;
            
            // TODO NotImplemented
        }
    }

    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class AvoidSmallNamespaceHighlighting : IHighlighting
    {
        public ICSharpNamespaceDeclaration Declaration { get; private set; }

        public AvoidSmallNamespaceHighlighting(ICSharpNamespaceDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Design: avoid small namespace."; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
#endif
}