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
using RGendarme.Settings.Naming;

namespace RGendarme.Rules.Naming.AvoidNonAlphanumericIdentifier
{
    [ElementProblemAnalyzer(new[] { typeof(ICSharpDeclaration) }, HighlightingTypes = new[] { typeof(AvoidNonAlphanumericIdentifierHighlighting) })]
    public class AvoidNonAlphanumericIdentifierAnalyzer : ElementProblemAnalyzer<ICSharpDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        public AvoidNonAlphanumericIdentifierAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(ICSharpDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            var namespaceDecl = element as ICSharpNamespaceDeclaration;
            if (namespaceDecl != null)
            {
                if (!CheckName(namespaceDecl.DeclaredName, false) && namespaceDecl.NameIdentifier != null)
                {
                    consumer.AddHighlighting(new AvoidNonAlphanumericIdentifierHighlighting(namespaceDecl), namespaceDecl.NameIdentifier.GetDocumentRange(), namespaceDecl.GetContainingFile());
                }
            }

            var methodDecl = element as IMethodDeclaration;
            if (methodDecl != null && methodDecl.IsPublic())
            {
                if (!CheckName(methodDecl.DeclaredName, false) && methodDecl.NameIdentifier != null)
                {
                    consumer.AddHighlighting(new AvoidNonAlphanumericIdentifierHighlighting(methodDecl), methodDecl.NameIdentifier.GetDocumentRange(), methodDecl.GetContainingFile());
                }
            }
        }

        #region Private methods (from Gendarme)
        // Compiler generates an error for any other non alpha-numerics than underscore ('_'), 
        // so we just need to check the presence of underscore in method names
        private static bool CheckName(string name, bool special)
        {
            int start = special ? name.IndexOf('_') + 1 : 0;
            return (name.IndexOf('_', start) == -1);
        }
        #endregion

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<NamingRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.AvoidNonAlphanumericIdentifierEnabled;
        }
    }
}