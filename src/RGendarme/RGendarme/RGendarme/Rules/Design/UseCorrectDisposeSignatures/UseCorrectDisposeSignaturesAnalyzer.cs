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

namespace RGendarme.Rules.Design.UseCorrectDisposeSignatures
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(UseCorrectDisposeSignaturesHighlighting) })]
    public class UseCorrectDisposeSignaturesAnalyzer : ElementProblemAnalyzer<IClassDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        public UseCorrectDisposeSignaturesAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            // 1. have to implement IDisposable interface
//            if (!AnalyzerHelper.IsImplement(element, "System.IDisposable"))
            if (!AnalyzerHelper.IsImplement(element, typeof(IDisposable)))
                return;

            // 2.  IDisposable type’s Dispose methods should either be nullary or unary with a bool argument
            foreach (IMethodDeclaration method in element.MethodDeclarationsEnumerable)
            {
                if (method.NameIdentifier == null || string.IsNullOrEmpty(method.DeclaredName) || !method.DeclaredName.Equals("Dispose") || method.ParameterDeclarations.IsEmpty)
                    continue;

#warning instead of comare type as strings use IsImplicitlyContertibleTo
                // TODO use PredifinedClrTypes
                if (method.ParameterDeclarations.Count > 1 || !method.ParameterDeclarations[0].Type.ToString().Equals(typeof(bool).FullName))
                {
                    consumer.AddHighlighting(new UseCorrectDisposeSignaturesHighlighting(element, "Dispose methods should either be nullary or unary with a bool argument."), method.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
                    continue;
                }
            }

            // 3. Dispose () should not be virtual
            IMethodDeclaration virtualDispose = element.MethodDeclarationsEnumerable.FirstOrDefault(
                m => m.NameIdentifier != null
                     && !string.IsNullOrEmpty(m.DeclaredName)
                     && m.DeclaredName.Equals("Dispose")
                     && m.ParameterDeclarations.IsEmpty
                     && m.ModifiersList != null
                     && m.ModifiersList.HasModifier(CSharpTokenType.VIRTUAL_KEYWORD)
                );
            if (virtualDispose != null)
            {
                consumer.AddHighlighting(new UseCorrectDisposeSignaturesHighlighting(element, "Should not be virtual."), virtualDispose.NameIdentifier.GetDocumentRange(), virtualDispose.GetContainingFile());
            }

            // 4. Dispose (bool) should not be public
#warning instead of comare type as strings use IsImplicitlyContertibleTo
            IMethodDeclaration boolableDispose = element.MethodDeclarationsEnumerable.FirstOrDefault(
                m => m.NameIdentifier != null
                     && !string.IsNullOrEmpty(m.DeclaredName)
                     && m.DeclaredName.Equals("Dispose")
                     && m.ParameterDeclarations.Count == 1
                     && m.ParameterDeclarations[0].Type.ToString().Equals(typeof(bool).FullName)
                     && m.ModifiersList != null
                     && m.ModifiersList.HasModifier(CSharpTokenType.PUBLIC_KEYWORD)
                );
            if (boolableDispose != null)
            {
                consumer.AddHighlighting(new UseCorrectDisposeSignaturesHighlighting(element, "Should not be public"), boolableDispose.NameIdentifier.GetDocumentRange(), boolableDispose.GetContainingFile());
            }

            // 5. unsealed types should have a protected virtual Dispose (bool) method
#warning instead of comare type as strings use IsImplicitlyContertibleTo
            bool isSealed = element.ModifiersList != null &&
                            element.ModifiersList.HasModifier(CSharpTokenType.SEALED_KEYWORD);
            if (!isSealed)
            {
                IMethodDeclaration disposeInSealed = element.MethodDeclarationsEnumerable.FirstOrDefault(
                    m => m.NameIdentifier != null
                         && !string.IsNullOrEmpty(m.DeclaredName)
                         && m.DeclaredName.Equals("Dispose")
                         && m.ParameterDeclarations.Count == 1
                         && m.ParameterDeclarations[0].Type.ToString().Equals(typeof(bool).FullName)
                         &&
                         (m.ModifiersList == null || !m.ModifiersList.HasModifier(CSharpTokenType.PROTECTED_KEYWORD) ||
                          !m.ModifiersList.HasModifier(CSharpTokenType.VIRTUAL_KEYWORD)));
                if (disposeInSealed != null)
                {
                    consumer.AddHighlighting(new UseCorrectDisposeSignaturesHighlighting(element, "Should be protected virtual."), disposeInSealed.NameIdentifier.GetDocumentRange(), disposeInSealed.GetContainingFile());
                }
            }
        }

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<DesignRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.UseCorrectDisposeSignaturesEnabled;
        }
    }
}