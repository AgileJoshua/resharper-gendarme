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
                if (body.Methods.IsEmpty && body.Properties.IsEmpty && body.EventDeclarations.IsEmpty && body.Indexers.IsEmpty)
                {
                    isEmptyInterface = true;
                }
            }

            if (isEmptyInterface)
            {
                ICSharpIdentifier interfaceName = element.NameIdentifier;
                consumer.AddHighlighting(new AvoidEmptyInterfaceHighlighting(interfaceName), interfaceName.GetDocumentRange(), interfaceName.GetContainingFile());
            }
        }
    }

    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class AvoidEmptyInterfaceHighlighting : IHighlighting
    {
        public ICSharpIdentifier Declaration { get; private set; }
        public AvoidEmptyInterfaceHighlighting(ICSharpIdentifier declaration)
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