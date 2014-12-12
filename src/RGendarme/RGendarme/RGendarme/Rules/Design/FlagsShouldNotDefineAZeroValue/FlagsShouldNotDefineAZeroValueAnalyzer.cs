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

namespace RGendarme.Rules.Design.FlagsShouldNotDefineAZeroValue
{
    [ElementProblemAnalyzer(new[] { typeof(IEnumDeclaration) }, HighlightingTypes = new[] { typeof(FlagsShouldNotDefineAZeroValueHighlighting) })]
    public class FlagsShouldNotDefineAZeroValueAnalyzer : ElementProblemAnalyzer<IEnumDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        public FlagsShouldNotDefineAZeroValueAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IEnumDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;
            
            // 1. check for Flags attribute
            if (!AnalyzerHelper.HasAttribute(element, "System.FlagsAttribute"))
                return;

            // 2. check for zero value
            IEnumMemberDeclaration zeroMember = null;
            foreach (IEnumMemberDeclaration item in element.EnumMemberDeclarationsEnumerable)
            {
                var expr = item.ValueExpression as ICSharpLiteralExpression;
                if (expr == null) continue;

                ConstantValue val = expr.ConstantValue;
                if (val.IsZero())
                {
                    zeroMember = item;
                    break;
                }
            }

            if (zeroMember != null)
            {
                consumer.AddHighlighting(new FlagsShouldNotDefineAZeroValueHighlighting(element), zeroMember.GetDocumentRange(), element.GetContainingFile());
            }
        }

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<DesignRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.FlagsShouldNotDefineAZeroValueEnabled;
        }
    }
}