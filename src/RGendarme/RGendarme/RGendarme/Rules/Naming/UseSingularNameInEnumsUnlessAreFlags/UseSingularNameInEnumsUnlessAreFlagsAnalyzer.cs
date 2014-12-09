using System.Globalization;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib;

namespace RGendarme.Rules.Naming.UseSingularNameInEnumsUnlessAreFlags
{
    [ElementProblemAnalyzer(new[] { typeof(IEnumDeclaration) }, HighlightingTypes = new[] { typeof(UseSingularNameInEnumsUnlessAreFlagsHighlighting) })]
    public class UseSingularNameInEnumsUnlessAreFlagsAnalyzer : ElementProblemAnalyzer<IEnumDeclaration>
    {
        private readonly ISettingsStore _settings;

        public UseSingularNameInEnumsUnlessAreFlagsAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IEnumDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (AnalyzerHelper.HasAttribute(element, "System.FlagsAttribute") || element.NameIdentifier == null)
                return;

            string name = element.DeclaredName;
            if (!string.IsNullOrEmpty(name) && IsPlural(name))
            {
                consumer.AddHighlighting(new UseSingularNameInEnumsUnlessAreFlagsHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }
        }

        private static bool IsPlural (string typeName)
		{
			return (string.Compare (typeName, typeName.Length - 1, "s", 0, 1, true, CultureInfo.CurrentCulture) == 0);
		}
    }
}