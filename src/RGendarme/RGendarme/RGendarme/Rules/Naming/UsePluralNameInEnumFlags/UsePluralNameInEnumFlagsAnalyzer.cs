using System;
using System.Globalization;
using JetBrains.Application.DataContext;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib;
using RGendarme.Settings.Naming;

namespace RGendarme.Rules.Naming.UsePluralNameInEnumFlags
{
    [ElementProblemAnalyzer(new[] { typeof(IEnumDeclaration) }, HighlightingTypes = new[] { typeof(UsePluralNameInEnumFlagsHighlighting) })]
    public class UsePluralNameInEnumFlagsAnalyzer : ElementProblemAnalyzer<IEnumDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        public UsePluralNameInEnumFlagsAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IEnumDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            if (!AnalyzerHelper.HasAttribute(element, "System.FlagsAttribute") || element.NameIdentifier == null)
                return;

            string name = element.DeclaredName;
            if (!string.IsNullOrEmpty(name) && !IsPlural(name))
            {
                consumer.AddHighlighting(new UsePluralNameInEnumFlagsHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }
        }

        private static bool IsPlural(string typeName)
        {
            return (string.Compare(typeName, typeName.Length - 1, "s", 0, 1, true, CultureInfo.CurrentCulture) == 0);
        }

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<NamingRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.UsePluralNameInEnumFlagsEnabled;
        }
    }
}