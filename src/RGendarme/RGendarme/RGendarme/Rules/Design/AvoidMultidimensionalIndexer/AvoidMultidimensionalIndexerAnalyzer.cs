using System;
using JetBrains.Application.DataContext;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib;
using RGendarme.Settings.Design;

namespace RGendarme.Rules.Design.AvoidMultidimensionalIndexer
{
    [ElementProblemAnalyzer(new[] { typeof(IIndexerDeclaration) }, HighlightingTypes = new[] { typeof(AvoidMultidimensionalIndexerHighlighting) })]
    public class AvoidMultidimensionalIndexerAnalyzer : ElementProblemAnalyzer<IIndexerDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        public AvoidMultidimensionalIndexerAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IIndexerDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            IFormalParameterList parameters = element.Params;
            if (parameters == null) return;

            if (parameters.ParameterDeclarations.Count > 1)
            {
                var thisKeyword = element.ThisKeyword;

                consumer.AddHighlighting(new AvoidMultidimensionalIndexerHighlighting(element), thisKeyword.GetDocumentRange(), thisKeyword.GetContainingFile());    
            }
        }

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<DesignRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.AvoidMultidimensionalIndexerEnabled;
        }
    }
}