using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Design.AvoidEmptyInterface
{
    [ElementProblemAnalyzer(new[] { typeof(IInterfaceDeclaration) }, HighlightingTypes = new[] { typeof(AvoidEmptyInterfaceHighlighting) })]
    public class AvoidEmptyInterfaceAnalyzer : ElementProblemAnalyzer<IInterfaceDeclaration>
    {
        protected override void Run(IInterfaceDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            bool isEmptyInterface = false;

            IClassBody body = element.Body;
            if (body == null)
                isEmptyInterface = true;

            if (!isEmptyInterface)
            {
                if (body.Methods.Count == 0)
                    isEmptyInterface = true;
            }

            if (isEmptyInterface)
            {
                consumer.AddHighlighting(new AvoidEmptyInterfaceHighlighting(element), element.GetDocumentRange(), element.GetContainingFile());
            }
        }
    }

    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class AvoidEmptyInterfaceHighlighting : IHighlighting
    {
        public IInterfaceDeclaration Declaration { get; private set; }
        public AvoidEmptyInterfaceHighlighting(IInterfaceDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Empty interface - use attribute instead"; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}