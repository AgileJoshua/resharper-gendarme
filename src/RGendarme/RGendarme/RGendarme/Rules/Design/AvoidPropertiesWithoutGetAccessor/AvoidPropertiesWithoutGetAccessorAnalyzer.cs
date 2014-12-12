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

namespace RGendarme.Rules.Design.AvoidPropertiesWithoutGetAccessor
{
    [ElementProblemAnalyzer(new[] { typeof(IPropertyDeclaration) }, HighlightingTypes = new[] { typeof(AvoidPropertiesWithoutGetAccessorHighlighting) })]
    public class AvoidPropertiesWithoutGetAccessorAnalyzer : ElementProblemAnalyzer<IPropertyDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        public AvoidPropertiesWithoutGetAccessorAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IPropertyDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            bool isGetDefined = false;

#warning refator it 
            // TODO: use AccessorDeclarationsEnumerable.FirstOrDefault(accessor => accessor.Kind == AccessorKind.GETTER);
            foreach (IAccessorDeclaration accessor in element.AccessorDeclarationsEnumerable)
            {
                if (accessor.NameIdentifier == null) continue;

                string name = accessor.NameIdentifier.Name;
                if (!string.IsNullOrEmpty(name) && name.Equals("get"))
                {
                    isGetDefined = true;
                    break;
                }
            }

            if (!isGetDefined)
            {
                consumer.AddHighlighting(new AvoidPropertiesWithoutGetAccessorHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }
        }

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<DesignRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.AvoidPropertiesWithoutGetAccessorEnabled;
        }
    }
}