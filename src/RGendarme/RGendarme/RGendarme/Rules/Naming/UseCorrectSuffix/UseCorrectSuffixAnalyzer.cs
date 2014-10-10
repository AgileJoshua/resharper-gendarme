using System;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Naming.UseCorrectSuffix
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(UseCorrectSuffixHighlighting) })]
    public class UseCorrectSuffixAnalyzer : ElementProblemAnalyzer<IClassDeclaration>
    {
        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            Analyze(element, consumer, "Attribute", (id, suffix) => new UseCorrectSuffixHighlighting(id, suffix));
            Analyze(element, consumer, "EventArgs", (id, suffix) => new UseCorrectSuffixHighlighting(id, suffix));
        }

        private void Analyze(IClassDeclaration element, IHighlightingConsumer consumer, string suffix,
            Func<ICSharpIdentifier, string, IHighlighting> highlighting)
        {
            AnalyzeWrongClassName(element, consumer, suffix, highlighting);
            AnalyzeNotImplement(element, consumer, suffix, highlighting);
        }

        private void AnalyzeWrongClassName(IClassDeclaration element, IHighlightingConsumer consumer, string suffix, Func<ICSharpIdentifier, string, IHighlighting> highlighting)
        {
            string errorMsg = string.Format("Class doesn't have {0} suffix", suffix);

            // 1. get exntend list
            IExtendsList extends = element.ExtendsList;
            if (extends == null)
                return;

            // 2. check if class extends Attribute class, it has to have Attbite suffix
            bool isExtendAttribute = false;
            foreach (IDeclaredTypeUsage type in extends.ExtendedInterfaces)
            {
                var declaredType = type as IUserDeclaredTypeUsage;
                if (declaredType != null)
                {
                    if (declaredType.TypeName != null)
                    {
                        string extendTypeName = declaredType.TypeName.ShortName;
                        if (extendTypeName.Equals(suffix))
                            isExtendAttribute = true;
                    }
                }
            }

            if (!isExtendAttribute)
                return;

            // 3. get class name
            string name = element.NameIdentifier.Name;
            if (!string.IsNullOrEmpty(name) && !name.EndsWith(suffix))
            {
                ICSharpIdentifier identifier = element.NameIdentifier;
                consumer.AddHighlighting(highlighting(identifier, errorMsg), identifier.GetDocumentRange(), identifier.GetContainingFile());
            }
        }

        private void AnalyzeNotImplement(IClassDeclaration element, IHighlightingConsumer consumer, string suffix,
            Func<ICSharpIdentifier, string, IHighlighting> highlighting)
        {
            string errorMsg = string.Format("Has {0} suffix but doesn't extend {0} class", suffix); 

            // 1. get class name
            string name = element.NameIdentifier.Name;

            if (!string.IsNullOrEmpty(name) && !name.EndsWith(suffix))
                return;

            // 2. get exntend list
            IExtendsList extends = element.ExtendsList;
            if (extends == null)
            {
                ICSharpIdentifier nameIdentifier = element.NameIdentifier;
                consumer.AddHighlighting(highlighting(nameIdentifier, errorMsg), nameIdentifier.GetDocumentRange(), nameIdentifier.GetContainingFile());
                return;
            }

            // 3. check if class has Attribute suffix but doesn't extend Attribute class
            bool isImplementAttribute = false;
            foreach (IDeclaredTypeUsage type in extends.ExtendedInterfaces)
            {
                var declaredType = type as IUserDeclaredTypeUsage;
                if (declaredType != null)
                {
                    if (declaredType.TypeName != null)
                    {
                        string extendTypeName = declaredType.TypeName.ShortName;
                        if (extendTypeName.Equals(suffix))
                            isImplementAttribute = true;
                    }
                }
            }

            if (!isImplementAttribute)
            {
                ICSharpIdentifier nameIdentifier = element.NameIdentifier;
                consumer.AddHighlighting(highlighting(nameIdentifier, errorMsg), nameIdentifier.GetDocumentRange(), nameIdentifier.GetContainingFile());
            }
        }
    }
}