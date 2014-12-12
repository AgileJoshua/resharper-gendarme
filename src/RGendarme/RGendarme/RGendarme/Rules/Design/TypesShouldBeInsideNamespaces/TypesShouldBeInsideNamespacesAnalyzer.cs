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

namespace RGendarme.Rules.Design.TypesShouldBeInsideNamespaces
{
    [ElementProblemAnalyzer(new[] { typeof(ICSharpTypeDeclaration) }, HighlightingTypes = new[] { typeof(TypesShouldBeInsideNamespacesHighlighting) })]
    public class TypesShouldBeInsideNamespacesAnalyzer : ElementProblemAnalyzer<ICSharpTypeDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        public TypesShouldBeInsideNamespacesAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(ICSharpTypeDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            ICSharpNamespaceDeclaration namespaceDecl = element.GetContainingNamespaceDeclaration();
            if (namespaceDecl == null && element.NameIdentifier != null)
            {
                consumer.AddHighlighting(new TypesShouldBeInsideNamespacesHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }
        }

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<DesignRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.TypesShouldBeInsideNamespacesEnabled;
        }
    }
}