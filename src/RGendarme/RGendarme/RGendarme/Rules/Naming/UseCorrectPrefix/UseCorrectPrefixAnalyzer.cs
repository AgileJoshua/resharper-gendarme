using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Naming.UseCorrectPrefix
{
    /// <summary>
    /// 
    /// </summary>
    /// <note>
    /// Interface rule already implemented in R#.
    /// </note>
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(UseCorrectPrefixHighlighting), typeof(UseCorrectPrefixWrongGenericTypeNameHighlighting) })]
    public class UseCorrectPrefixAnalyzer : ElementProblemAnalyzer<IClassDeclaration>
    {
        private readonly ISettingsStore _settings;

        public UseCorrectPrefixAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            AnalyzeCPrefix(element, consumer);
            AnalyzeGenericType(element, consumer);
        }

        private void AnalyzeGenericType(IClassDeclaration element, IHighlightingConsumer consumer)
        {
            ITypeParameterOfTypeList genericParams = element.TypeParameterList;
            if (genericParams == null)
                return;

            foreach (ITypeParameterOfTypeDeclaration param in genericParams.TypeParameterDeclarationsEnumerable)
            {
                if (param.NameIdentifier == null || string.IsNullOrEmpty(param.NameIdentifier.Name))
                    continue;

                string name = param.NameIdentifier.Name;
                if (!name[0].Equals('T'))
                {
                    consumer.AddHighlighting(new UseCorrectPrefixWrongGenericTypeNameHighlighting(param), param.GetDocumentRange(), param.GetContainingFile());
                }
            }
        }

        private void AnalyzeCPrefix(IClassDeclaration element, IHighlightingConsumer consumer)
        {
            if (element.NameIdentifier == null || string.IsNullOrEmpty(element.NameIdentifier.Name))
                return;

            string name = element.NameIdentifier.Name;

            if (name[0].Equals('C') && name.Length > 1)
            {
                char second = name[1];
                if (char.IsUpper(second))
                {
                    consumer.AddHighlighting(new UseCorrectPrefixHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
                }
            }
        }
    }
}