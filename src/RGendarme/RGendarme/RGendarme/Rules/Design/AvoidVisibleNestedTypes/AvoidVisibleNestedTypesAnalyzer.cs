using System;
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

namespace RGendarme.Rules.Design.AvoidVisibleNestedTypes
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(AvoidVisibleNestedTypesHighlighting) })]
    public class AvoidVisibleNestedTypesAnalyzer : ElementProblemAnalyzer<IClassDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;
        public AvoidVisibleNestedTypesAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            if (element.Body == null || element.Body.TypeDeclarations.Count == 0)
                return;

            bool hasNestedPublicClass = false;
            foreach (ICSharpTypeDeclaration declaration in element.Body.TypeDeclarationsEnumerable)
            {
                var customTypeDeclaration = declaration as IClassDeclaration;
                if (customTypeDeclaration == null)
                    continue;

                IModifiersList modifiers = customTypeDeclaration.ModifiersList;
                if (modifiers == null)
                    continue;

                foreach (ITokenNode mod in modifiers.ModifiersEnumerable)
                {
                    if (mod.NodeType == CSharpTokenType.PUBLIC_KEYWORD)
                    {
                        hasNestedPublicClass = true;
                        break;
                    }
                }

                if (hasNestedPublicClass)
                    break;
            }

            if (hasNestedPublicClass)
            {
                consumer.AddHighlighting(new AvoidVisibleNestedTypesHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }
        }

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<DesignRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.AvoidVisibleNestedTypesEnabled;
        }
    }
}