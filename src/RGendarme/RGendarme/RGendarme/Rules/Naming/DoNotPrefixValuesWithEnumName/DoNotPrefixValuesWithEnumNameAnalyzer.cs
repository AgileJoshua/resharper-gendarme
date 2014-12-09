using System;
using JetBrains.Application.DataContext;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib;
using RGendarme.Settings.Naming;

namespace RGendarme.Rules.Naming.DoNotPrefixValuesWithEnumName
{
    [ElementProblemAnalyzer(new[] { typeof(IEnumDeclaration) }, HighlightingTypes = new[] { typeof(DoNotPrefixValuesWithEnumNameHighlighting) })]
    public class DoNotPrefixValuesWithEnumNameAnalyzer : ElementProblemAnalyzer<IEnumDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        public DoNotPrefixValuesWithEnumNameAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IEnumDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

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

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<NamingRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.DoNotPrefixValuesWithEnumNameEnabled;
        }
    }
}