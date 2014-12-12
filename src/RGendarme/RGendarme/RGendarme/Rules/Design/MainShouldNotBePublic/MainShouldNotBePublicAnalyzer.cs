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

namespace RGendarme.Rules.Design.MainShouldNotBePublic
{
    [ElementProblemAnalyzer(new []{typeof(IMethodDeclaration)}, HighlightingTypes = new []{typeof(MainShouldNotBePublicHighlighting)})]
    public class MainShouldNotBePublicAnalyzer : ElementProblemAnalyzer<IMethodDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        public MainShouldNotBePublicAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IMethodDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            // 1. if method is not main - exit
            if (!element.IsValid() || element.NameIdentifier == null || string.IsNullOrEmpty(element.DeclaredName) || string.Compare(element.DeclaredName, "main", StringComparison.OrdinalIgnoreCase) != 0)
                return;

            // 2. is the main method public
            bool isMainPublic = element.ModifiersList != null && element.ModifiersList.HasModifier(CSharpTokenType.PUBLIC_KEYWORD);

            ICSharpTypeDeclaration containingType = element.GetContainingTypeDeclaration();
            if (containingType == null)
                return;

            // 3. is the containing type public
            bool isTypePublic = containingType.ModifiersList != null &&
                                containingType.ModifiersList.HasModifier(CSharpTokenType.PUBLIC_KEYWORD);

            if (isMainPublic && isTypePublic)
            {
                consumer.AddHighlighting(new MainShouldNotBePublicHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }
        }

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<DesignRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.MainShouldNotBePublicEnabled;
        }
    }
}