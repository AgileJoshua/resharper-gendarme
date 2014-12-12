using System;
using JetBrains.Application.DataContext;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib;
using RGendarme.Settings.Design;

namespace RGendarme.Rules.Design.ImplementICloneableCorrectly
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(ImplementICloneableCorrectlyHighlighting) })]
    public class ImplementICloneableCorrectlyAnalyzer : ElementProblemAnalyzer<IClassDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        public ImplementICloneableCorrectlyAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            // 1. Does class have 'object Clone()' method?
            bool hasCloneMethod = false;
            foreach (IMethodDeclaration method in element.MethodDeclarationsEnumerable)
            {
                // checking method name
                if (method.NameIdentifier == null || string.IsNullOrEmpty(method.NameIdentifier.Name) || !method.NameIdentifier.Name.Equals("Clone"))
                    continue;

                // checking return type
                if (method.DeclaredElement == null || !method.DeclaredElement.ReturnType.IsObject())
                    continue;

                if (method.ParameterDeclarations.Count == 0)
                {
                    hasCloneMethod = true;
                    break;
                }
            }

            // 1. Does it implement ICloneable
            if (hasCloneMethod && !AnalyzerHelper.IsImplement(element, "System.ICloneable"))
            {
                consumer.AddHighlighting(new ImplementICloneableCorrectlyHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }
        }

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<DesignRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.ImplementICloneableCorrectlyEnabled;
        }
    }
}