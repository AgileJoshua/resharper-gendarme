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

namespace RGendarme.Rules.Design.PreferEventsOverMethods
{
    [ElementProblemAnalyzer(new[] { typeof(IMethodDeclaration) }, HighlightingTypes = new[] { typeof(PreferEventsOverMethodsHighlighting) })]
    public class PreferEventsOverMethodsAnalyzer : ElementProblemAnalyzer<IMethodDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        private readonly static string[] SuspectPrefixes =
        {
            "AddOn",
            "RemoveOn",
            "Fire",
            "Raise"
        };

        public PreferEventsOverMethodsAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IMethodDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            string name = element.DeclaredName;
            if (element.NameIdentifier == null || string.IsNullOrEmpty(name))
                return;
            
            if (SuspectPrefixes.Any(prefix => name.StartsWith(prefix, StringComparison.Ordinal)))
            {
                consumer.AddHighlighting(new PreferEventsOverMethodsHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }
        }

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<DesignRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.PreferEventsOverMethodsEnabled;
        }
    }
}