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

namespace RGendarme.Rules.Design.AvoidVisibleFields
{
    [ElementProblemAnalyzer(new[] { typeof(IMultipleFieldDeclaration) }, HighlightingTypes = new[] { typeof(AvoidVisibleFieldsHighlighting) })]
    public class  AvoidVisibleFieldsAnalyzer : ElementProblemAnalyzer<IMultipleFieldDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        public AvoidVisibleFieldsAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IMultipleFieldDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            bool isPublicField = false;
            IModifiersList modifiers = element.ModifiersList;

            if (modifiers == null) // if no modifiers - field is private by default
                 return;
            
#warning it's bad pracise
            // TODO refactor it using NodeType checking
            foreach (ITokenNode m in modifiers.Modifiers)
            {
                if (m.GetText().Equals("public"))
                    isPublicField = true;
            }

            if (isPublicField)
            {
                consumer.AddHighlighting(new AvoidVisibleFieldsHighlighting(element), element.GetDocumentRange(), element.GetContainingFile());
            }
        }

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<DesignRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.AvoidVisibleFieldsEnabled;
        }
    }
}