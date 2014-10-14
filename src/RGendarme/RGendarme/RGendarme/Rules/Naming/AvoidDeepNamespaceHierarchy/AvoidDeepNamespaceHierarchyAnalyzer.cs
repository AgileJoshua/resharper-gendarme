using System;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Naming.AvoidDeepNamespaceHierarchy
{
    [ElementProblemAnalyzer(new[] { typeof(ICSharpNamespaceDeclaration) }, HighlightingTypes = new[] { typeof(AvoidDeepNamespaceHierarchyHighlighting) })]
    public class AvoidDeepNamespaceHierarchyAnalyzer : ElementProblemAnalyzer<ICSharpNamespaceDeclaration>
    {
        private const int MaxDeepLevel = 4;

        protected override void Run(ICSharpNamespaceDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {   
            if (element.NameIdentifier == null) return;

            string name = element.NameIdentifier.Name;
            if (string.IsNullOrEmpty(name)) return;

            IOwnerQualification parent = element.NamespaceQualification;
            if (parent == null) return;

            IReferenceName parentQualifier = parent.Qualifier;
            if (parentQualifier.NameIdentifier == null)
                return;

            string fullName = parentQualifier.GetText() + "." + name;
            string[] parts = fullName.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > MaxDeepLevel)
            {
                consumer.AddHighlighting(new AvoidDeepNamespaceHierarchyHighlighting(element, MaxDeepLevel), element.NamespaceQualification.GetDocumentRange(), element.GetContainingFile());
            }
        }
    }
}