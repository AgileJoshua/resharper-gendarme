using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib;

namespace RGendarme.Rules.Design.MissingAttributeUsageOnCustomAttribute
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(MissingAttributeUsageOnCustomAttributeHighlighting) })]
    public class MissingAttributeUsageOnCustomAttributeAnalyzer : ElementProblemAnalyzer<IClassDeclaration>
    {
        private readonly ISettingsStore _settings;

        public MissingAttributeUsageOnCustomAttributeAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            IExtendsList extends = element.ExtendsList;
            if (extends == null) 
                return;

            if (!AnalyzerHelper.IsImplement(extends, "System.Attribute"))
                return;

            if (!AnalyzerHelper.HasAttribute(element, "System.AttributeUsageAttribute"))
            {
                consumer.AddHighlighting(new MissingAttributeUsageOnCustomAttributeHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }
        }
    }
}