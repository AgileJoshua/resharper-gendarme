using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Naming.DoNotPrefixValuesWithEnumName
{
    [ElementProblemAnalyzer(new[] { typeof(IEnumDeclaration) }, HighlightingTypes = new[] { typeof(DoNotPrefixValuesWithEnumNameHighlighting) })]
    public class DoNotPrefixValuesWithEnumNameAnalyzer : ElementProblemAnalyzer<IEnumDeclaration>
    {
        private readonly ISettingsStore _settings;

        public DoNotPrefixValuesWithEnumNameAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IEnumDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            ICSharpIdentifier enumIdentifier = element.NameIdentifier;
            if (enumIdentifier == null) return;

            string enumId = enumIdentifier.Name;
            if (string.IsNullOrEmpty(enumId)) return;
            
            IEnumBody body = element.EnumBody;
            if (body == null) return;

            if (body.Members.Count == 0) return;

            foreach (IEnumMemberDeclaration item in body.MembersEnumerable)
            {
                if (item.NameIdentifier == null) continue;

                string itemId = item.NameIdentifier.Name;
                if (string.IsNullOrEmpty(itemId)) continue;

                if (itemId.StartsWith(enumId))
                {
                    consumer.AddHighlighting(new DoNotPrefixValuesWithEnumNameHighlighting(item), item.GetDocumentRange(), item.GetContainingFile());
                }
            }
        }
    }
}