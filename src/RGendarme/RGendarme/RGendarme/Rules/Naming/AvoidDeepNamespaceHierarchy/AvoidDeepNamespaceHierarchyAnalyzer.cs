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

namespace RGendarme.Rules.Naming.AvoidDeepNamespaceHierarchy
{
    [ElementProblemAnalyzer(new[] { typeof(ICSharpNamespaceDeclaration) }, HighlightingTypes = new[] { typeof(AvoidDeepNamespaceHierarchyHighlighting) })]
    public class AvoidDeepNamespaceHierarchyAnalyzer : ElementProblemAnalyzer<ICSharpNamespaceDeclaration>, IRGendarmeRule
    {
        private const int MaxDeepLevel = 4;

        private readonly ISettingsStore _settings;
        
        public AvoidDeepNamespaceHierarchyAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(ICSharpNamespaceDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            if (element.NameIdentifier == null) return;

            string name = element.NameIdentifier.Name;
            if (string.IsNullOrEmpty(name)) return;

            IOwnerQualification parent = element.NamespaceQualification;
            if (parent == null) return;

            IReferenceName parentQualifier = parent.Qualifier;
            if (parentQualifier.NameIdentifier == null)
                return;

            string fullName = parentQualifier.GetText() + "." + name;
            string[] parts = fullName.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > MaxDeepLevel)
            {
                consumer.AddHighlighting(new AvoidDeepNamespaceHierarchyHighlighting(element, MaxDeepLevel), element.NamespaceQualification.GetDocumentRange(), element.GetContainingFile());
            }
        }


        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<NamingRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.AvoidDeepNamespaceHierarchyEnabled;
        }
    }
}