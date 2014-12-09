using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Design.PreferIntegerOrStringForIndexers
{
    [ElementProblemAnalyzer(new[] { typeof(IIndexerDeclaration) }, HighlightingTypes = new[] { typeof(PreferIntegerOrStringForIndexersHighlighting) })]
    public class PreferIntegerOrStringForIndexersAnalyzer : ElementProblemAnalyzer<IIndexerDeclaration>
    {
        private readonly ISettingsStore _settings;

        public PreferIntegerOrStringForIndexersAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IIndexerDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            IFormalParameterList inputList = element.Params;
            if (inputList == null) 
                return;

            foreach (ICSharpParameterDeclaration param in inputList.ParameterDeclarationsEnumerable)
            {
                IParameter p = param.DeclaredElement;
                if (p == null) continue;

                IType type = p.Type;
                if (type.IsInt() || type.IsString() || type.IsLong()) continue;

                consumer.AddHighlighting(new PreferIntegerOrStringForIndexersHighlighting(element), param.GetDocumentRange(), param.GetContainingFile());
            }
        }
    }
}