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

namespace RGendarme.Rules.Design.ImplementEqualsAndGetHashCodeInPair
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(ImplementEqualsAndGetHashCodeInPairHighlighting) })]
    public class ImplementEqualsAndGetHashCodeInPairAnalyzer : ElementProblemAnalyzer<IClassDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        public ImplementEqualsAndGetHashCodeInPairAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            if (element.MethodDeclarations.IsEmpty)
                return;

            IMethodDeclaration getHashCode = element.MethodDeclarations.FirstOrDefault(
                m => m.NameIdentifier != null
                    && m.DeclaredElement != null
                    && !string.IsNullOrEmpty(m.DeclaredElement.ShortName)
                    && m.DeclaredElement.ShortName.Equals(CommonMethodName.GET_HASH_CODE)
                    && m.ParameterDeclarations.IsEmpty
                    && m.ModifiersList != null
                    && m.ModifiersList.HasModifier(CSharpTokenType.OVERRIDE_KEYWORD));

            IMethodDeclaration equals = element.MethodDeclarations.FirstOrDefault(
                m => m.DeclaredElement != null
                    && !string.IsNullOrEmpty(m.DeclaredElement.ShortName)
                    && m.DeclaredElement.ShortName.Equals(CommonMethodName.EQUALS)
                    && m.ModifiersList != null
                    && m.ModifiersList.HasModifier(CSharpTokenType.OVERRIDE_KEYWORD));

            // case when Equals implemented but GetHashCode not already implemented in R#
            if (getHashCode != null && equals == null)
            {
                consumer.AddHighlighting(new ImplementEqualsAndGetHashCodeInPairHighlighting(getHashCode), getHashCode.NameIdentifier.GetDocumentRange(), getHashCode.GetContainingFile());
            }   
        }

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<DesignRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.ImplementEqualsAndGetHashCodeInPairEnabled;
        }
    }
}