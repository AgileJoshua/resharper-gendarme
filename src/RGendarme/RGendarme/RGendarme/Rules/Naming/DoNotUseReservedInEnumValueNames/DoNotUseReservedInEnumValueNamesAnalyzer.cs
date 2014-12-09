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

namespace RGendarme.Rules.Naming.DoNotUseReservedInEnumValueNames
{
    [ElementProblemAnalyzer(new[] { typeof(IEnumDeclaration) }, HighlightingTypes = new[] { typeof(DoNotUseReservedInEnumValueNamesHighlighting) })]
    public class DoNotUseReservedInEnumValueNamesAnalyzer : ElementProblemAnalyzer<IEnumDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        public DoNotUseReservedInEnumValueNamesAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IEnumDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

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

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<NamingRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.DoNotUseReservedInEnumValueNamesEnabled;
        }
    }
}