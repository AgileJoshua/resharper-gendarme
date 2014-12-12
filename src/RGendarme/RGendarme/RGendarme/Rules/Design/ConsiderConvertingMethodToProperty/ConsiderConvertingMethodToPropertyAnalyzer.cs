using System;
using System.Linq;
using JetBrains.Application.DataContext;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib;
using RGendarme.Settings.Design;

namespace RGendarme.Rules.Design.ConsiderConvertingMethodToProperty
{
    [ElementProblemAnalyzer(new[] { typeof(IMethodDeclaration) }, HighlightingTypes = new[] { typeof(ConsiderConvertingMethodToPropertyHighlighting) })]
    public class ConsiderConvertingMethodToPropertyAnalyzer : ElementProblemAnalyzer<IMethodDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        public ConsiderConvertingMethodToPropertyAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IMethodDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {   
            if (!IsEnabled(element.ToDataContext()))
                return;

            if (element.NameIdentifier == null || string.IsNullOrEmpty(element.NameIdentifier.Name)) 
                return;

            // 1. Does method name start with prefixes below?
            var prefixes = new [] { "Get", "Set", "Is" };
            if (!prefixes.Any(prefix => element.NameIdentifier.Name.StartsWith(prefix)))
                return;

            // 2. parameters list is empty
            if (element.Params == null || element.Params.ParameterDeclarations.IsEmpty)
            {
                consumer.AddHighlighting(new ConsiderConvertingMethodToPropertyHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }
        }

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<DesignRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.ConsiderConvertingMethodToPropertyEnabled;
        }
    }
}