using System;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Naming.DoNotUseReservedInEnumValueNames
{
    [ElementProblemAnalyzer(new[] { typeof(IEnumDeclaration) }, HighlightingTypes = new[] { typeof(DoNotUseReservedInEnumValueNamesHighlighting) })]
    public class DoNotUseReservedInEnumValueNamesAnalyzer : ElementProblemAnalyzer<IEnumDeclaration>
    {
        private readonly ISettingsStore _settings;

        public DoNotUseReservedInEnumValueNamesAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IEnumDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (element.EnumMemberDeclarations.IsEmpty || element.NameIdentifier == null)
                return;

            foreach (IEnumMemberDeclaration item in element.EnumMemberDeclarationsEnumerable)
            {
                string name = item.DeclaredName;
                if (!string.IsNullOrEmpty(name) &&
                    string.Compare(name, "reserved", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    consumer.AddHighlighting(new DoNotUseReservedInEnumValueNamesHighlighting(element), item.GetDocumentRange(), item.GetContainingFile());
                }
            }
        }
    }
}