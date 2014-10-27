using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib;

namespace RGendarme.Rules.Design.ImplementICloneableCorrectly
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(ImplementICloneableCorrectlyHighlighting) })]
    public class ImplementICloneableCorrectlyAnalyzer : ElementProblemAnalyzer<IClassDeclaration>
    {
        private readonly ISettingsStore _settings;

        public ImplementICloneableCorrectlyAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            // 1. Does class have 'object Clone()' method?
            bool hasCloneMethod = false;
            foreach (IMethodDeclaration method in element.MethodDeclarationsEnumerable)
            {
                // checking method name
                if (method.NameIdentifier == null || string.IsNullOrEmpty(method.NameIdentifier.Name) || !method.NameIdentifier.Name.Equals("Clone"))
                    continue;

                // checking return type
                if (method.DeclaredElement == null || !method.DeclaredElement.ReturnType.IsObject())
                    continue;

                if (method.ParameterDeclarations.Count == 0)
                {
                    hasCloneMethod = true;
                    break;
                }
            }

            // 1. Does it implement ICloneable
            if (hasCloneMethod && !AnalyzerHelper.IsImplement(element, "System.ICloneable"))
            {
                consumer.AddHighlighting(new ImplementICloneableCorrectlyHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }
        }
    }
}