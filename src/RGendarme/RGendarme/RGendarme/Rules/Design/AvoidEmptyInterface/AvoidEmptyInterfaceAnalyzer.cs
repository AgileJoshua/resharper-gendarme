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

namespace RGendarme.Rules.Design.AvoidEmptyInterface
{
    [ElementProblemAnalyzer(new[] { typeof(IInterfaceDeclaration) }, HighlightingTypes = new[] { typeof(AvoidEmptyInterfaceHighlighting) })]
    public class AvoidEmptyInterfaceAnalyzer : ElementProblemAnalyzer<IInterfaceDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        public AvoidEmptyInterfaceAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IInterfaceDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            if (element.NameIdentifier == null || string.IsNullOrEmpty(element.NameIdentifier.Name))
                return;

            bool isEmptyInterface = false;

            IClassBody body = element.Body;
            if (body == null)
                isEmptyInterface = true;

            if (!isEmptyInterface)
            {
                if (body.Methods.IsEmpty && body.Properties.IsEmpty && body.EventDeclarations.IsEmpty && body.Indexers.IsEmpty)
                {
                    isEmptyInterface = true;
                }
            }

            if (isEmptyInterface)
            {
                ICSharpIdentifier interfaceName = element.NameIdentifier;
                consumer.AddHighlighting(new AvoidEmptyInterfaceHighlighting(interfaceName), interfaceName.GetDocumentRange(), interfaceName.GetContainingFile());
            }
        }

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<DesignRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.AvoidEmptyInterfaceEnabled;
        }
    }
}