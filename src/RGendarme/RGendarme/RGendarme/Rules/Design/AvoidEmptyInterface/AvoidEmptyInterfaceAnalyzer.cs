using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Design.AvoidEmptyInterface
{
    [ElementProblemAnalyzer(new[] { typeof(IInterfaceDeclaration) }, HighlightingTypes = new[] { typeof(AvoidEmptyInterfaceHighlighting) })]
    public class AvoidEmptyInterfaceAnalyzer : ElementProblemAnalyzer<IInterfaceDeclaration>
    {
        private readonly ISettingsStore _settings;

        public AvoidEmptyInterfaceAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IInterfaceDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (element.NameIdentifier == null || string.IsNullOrEmpty(element.NameIdentifier.Name))
                return;

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
}