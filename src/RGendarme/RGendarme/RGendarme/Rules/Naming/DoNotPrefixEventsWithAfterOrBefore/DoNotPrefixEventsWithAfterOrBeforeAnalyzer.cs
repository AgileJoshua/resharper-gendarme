using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Naming.DoNotPrefixEventsWithAfterOrBefore
{
    [ElementProblemAnalyzer(new[] { typeof(IEventDeclaration) }, HighlightingTypes = new[] { typeof(DoNotPrefixEventsWithAfterOrBeforeHighlighting) })]
    public class DoNotPrefixEventsWithAfterOrBeforeAnalyzer : ElementProblemAnalyzer<IEventDeclaration>
    {
        private readonly ISettingsStore _settings;

        public DoNotPrefixEventsWithAfterOrBeforeAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IEventDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            ICSharpIdentifier name = element.NameIdentifier;
            if (name == null) return;

            string id = name.GetText();
            if (string.IsNullOrEmpty(id)) return;

            if (id.StartsWith("After") || id.StartsWith("Before"))
            {
                consumer.AddHighlighting(new DoNotPrefixEventsWithAfterOrBeforeHighlighting(element), name.GetDocumentRange(), name.GetContainingFile());
            }
        }
    }
}