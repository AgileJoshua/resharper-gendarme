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

namespace RGendarme.Rules.Naming.DoNotPrefixEventsWithAfterOrBefore
{
    [ElementProblemAnalyzer(new[] { typeof(IEventDeclaration) }, HighlightingTypes = new[] { typeof(DoNotPrefixEventsWithAfterOrBeforeHighlighting) })]
    public class DoNotPrefixEventsWithAfterOrBeforeAnalyzer : ElementProblemAnalyzer<IEventDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        public DoNotPrefixEventsWithAfterOrBeforeAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IEventDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            ICSharpIdentifier name = element.NameIdentifier;
            if (name == null) return;

            string id = name.GetText();
            if (string.IsNullOrEmpty(id)) return;

            if (id.StartsWith("After") || id.StartsWith("Before"))
            {
                consumer.AddHighlighting(new DoNotPrefixEventsWithAfterOrBeforeHighlighting(element), name.GetDocumentRange(), name.GetContainingFile());
            }
        }

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<NamingRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.DoNotPrefixEventsWithAfterOrBeforeEnabled;
        }
    }
}