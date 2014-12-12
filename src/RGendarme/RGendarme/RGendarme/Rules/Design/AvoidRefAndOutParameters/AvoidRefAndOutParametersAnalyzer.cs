using System;
using JetBrains.Application.DataContext;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib;
using RGendarme.Lib.Extenstions;
using RGendarme.Settings.Design;

namespace RGendarme.Rules.Design.AvoidRefAndOutParameters
{
    [ElementProblemAnalyzer(new[] { typeof(IMethodDeclaration) }, HighlightingTypes = new[] { typeof(AvoidRefAndOutParametersHighlighting) })]
    public class AvoidRefAndOutParametersAnalyzer : ElementProblemAnalyzer<IMethodDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        public AvoidRefAndOutParametersAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IMethodDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            // 1. if method has not params - return
            IFormalParameterList parameters = element.Params;
            if (parameters == null || parameters.IsEmpty())
                return;

            // 2. if method has not ref or out params - return
            bool hasRefOrOut = false;
            foreach (ICSharpParameterDeclaration p in parameters.ParameterDeclarationsEnumerable)
            {
                if (p.IsRef() || p.IsOut())
                {
                    hasRefOrOut = true;
                    break;
                }
            }

            if (!hasRefOrOut)
                return;

            // 3. if method name starts with 'Try' - return
            if (element.NameIdentifier == null || string.IsNullOrEmpty(element.NameIdentifier.Name) || element.NameIdentifier.Name.StartsWith("Try"))
                return;

            // 4. else throw warning
            consumer.AddHighlighting(new AvoidRefAndOutParametersHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
        }

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<DesignRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.AvoidRefAndOutParametersEnabled;
        }
    }
}