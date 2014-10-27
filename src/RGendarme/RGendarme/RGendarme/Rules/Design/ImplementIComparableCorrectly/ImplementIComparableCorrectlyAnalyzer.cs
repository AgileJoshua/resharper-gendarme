using System;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib;

namespace RGendarme.Rules.Design.ImplementIComparableCorrectly
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(ImplementIComparableCorrectlyHighlight) })]
    public class ImplementIComparableCorrectlyAnalyzer : ElementProblemAnalyzer<IClassDeclaration>
    {
        private readonly ISettingsStore _settings;

        public ImplementIComparableCorrectlyAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            // 1. Does class implement IComparable
            if (element.ExtendsList == null || !AnalyzerHelper.IsImplement(element.ExtendsList, "System.IComparable"))
                return;

            // 2. Does it overrides Equals()
            bool isOvverrideEquals = false;
            foreach (IMethodDeclaration method in element.MethodDeclarationsEnumerable)
            {
                if (method.NameIdentifier != null && !string.IsNullOrEmpty(method.NameIdentifier.Name) &&
                    method.NameIdentifier.Name.Equals("Equals"))
                {
                    isOvverrideEquals = true;
                }

            }

            if (!isOvverrideEquals)
            {
                consumer.AddHighlighting(new ImplementIComparableCorrectlyHighlight(element, "Need override 'Equals' method."), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }

            // 3. Does it implement operators
            var neededOps = new[] { ">", "<", "==", "!=" };
            var ops = element.OperatorDeclarationsEnumerable.Select(op => op.DeclaredName).ToList();

            foreach (string op in neededOps)
            {
                if (ops.All(o => string.Compare(o, op, StringComparison.OrdinalIgnoreCase) != 0))
                {
                    consumer.AddHighlighting(new ImplementIComparableCorrectlyHighlight(element, string.Format("Need override {0} operator.", op)), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
                }
            }
        }
    }
}