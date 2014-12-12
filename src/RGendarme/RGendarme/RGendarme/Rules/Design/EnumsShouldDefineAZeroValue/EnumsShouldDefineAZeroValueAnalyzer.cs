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

namespace RGendarme.Rules.Design.EnumsShouldDefineAZeroValue
{
    [ElementProblemAnalyzer(new[] { typeof(IEnumDeclaration) }, HighlightingTypes = new[] { typeof(EnumsShouldDefineAZeroValueHighlighting) })]
    public class EnumsShouldDefineAZeroValueAnalyzer : ElementProblemAnalyzer<IEnumDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        public EnumsShouldDefineAZeroValueAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IEnumDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            if (AnalyzerHelper.HasAttribute(element, "System.FlagsAttribute"))
                return;

            bool hasZeroValue = false;
            foreach (IEnumMemberDeclaration item in element.EnumMemberDeclarationsEnumerable)
            {
                if (item.EnumMember == null || item.EnumMember.ConstantValue.Value == null)
                    continue;

                try
                {
                    int value = Convert.ToInt32(item.EnumMember.ConstantValue.Value);
                    if (value == 0)
                    {
                        hasZeroValue = true;
                        break;
                    }
                }
                catch (FormatException)
                {   
                }
            }

            if (!hasZeroValue)
            {
                consumer.AddHighlighting(new EnumsShouldDefineAZeroValueHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }

        }

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<DesignRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.EnumsShouldDefineAZeroValueEnabled;
        }
    }
}