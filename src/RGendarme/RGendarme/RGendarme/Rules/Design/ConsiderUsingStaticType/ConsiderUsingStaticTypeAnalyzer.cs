using System;
using System.Linq;
using JetBrains.Application.DataContext;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib;
using RGendarme.Settings.Design;

namespace RGendarme.Rules.Design.ConsiderUsingStaticType
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(ConsiderUsingStaticTypeHighlighting) })]
    public class ConsiderUsingStaticTypeAnalyzer : ElementProblemAnalyzer<IClassDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        public ConsiderUsingStaticTypeAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            if (element.Body == null)
                return;

            // 1. for non statis classes
            if (element.ModifiersList != null && element.ModifiersList.HasModifier(CSharpTokenType.STATIC_KEYWORD))
                return;

            bool onlyStaticMethods = !element.Body.Methods.IsEmpty && element.Body.MethodsEnumerable.All(m => m.IsStatic);

            bool onlyStaticFields = true;
            if (!element.Body.FieldDeclarations.IsEmpty)
            {
                foreach (IMultipleFieldDeclaration field in element.Body.FieldDeclarationsEnumerable)
                {
                    if (field.ModifiersList == null || !field.ModifiersList.HasModifier(CSharpTokenType.STATIC_KEYWORD))
                    {
                        onlyStaticFields = false;
                        break;
                    }
                }
            }

            bool onlyStaticProperties = !element.Body.Properties.IsEmpty &&
                                        element.Body.PropertiesEnumerable.All(p => p.IsStatic);

            bool onlyStaticEvents = true;
            if (!element.Body.EventDeclarations.IsEmpty)
            {
                foreach (IMultipleEventDeclaration evnt in element.Body.EventDeclarationsEnumerable)
                {
                    if (evnt.ModifiersList == null || !evnt.ModifiersList.HasModifier(CSharpTokenType.STATIC_KEYWORD))
                    {
                        onlyStaticEvents = false;
                        break;
                    }
                }
            }
            
            if (onlyStaticMethods && onlyStaticFields && onlyStaticProperties && onlyStaticEvents)
            {
                consumer.AddHighlighting(new ConsiderUsingStaticTypeHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }
        }

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<DesignRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.ConsiderUsingStaticTypeEnabled;
        }
    }
}