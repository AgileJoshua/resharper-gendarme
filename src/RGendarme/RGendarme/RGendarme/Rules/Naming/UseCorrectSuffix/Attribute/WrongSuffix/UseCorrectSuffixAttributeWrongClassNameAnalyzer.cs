using System;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Naming.UseCorrectSuffix.Attribute.WrongSuffix
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(UseCorrectSuffixAttributeWrongClassNameHighlighting) })]
    public class UseCorrectSuffixAttributeWrongClassNameAnalyzer : ElementProblemAnalyzer<IClassDeclaration>
    {
        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            Analyze(element, consumer, "Attribute", (id, suffix) => new UseCorrectSuffixAttributeWrongClassNameHighlighting(id, suffix));
            Analyze(element, consumer, "EventArgs", (id, suffix) => new UseCorrectSuffixAttributeWrongClassNameHighlighting(id, suffix));
        }

        private void Analyze(IClassDeclaration element, IHighlightingConsumer consumer, string suffix, Func<ICSharpIdentifier, string, IHighlighting> highlighting)
        {
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
                consumer.AddHighlighting(highlighting(identifier, suffix), identifier.GetDocumentRange(), identifier.GetContainingFile());
            }
        }
    }

    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class UseCorrectSuffixAttributeWrongClassNameHighlighting : IHighlighting
    {
        public  ICSharpIdentifier ClassDeclaration { get; private set; }
        private readonly string _suffix;

        public UseCorrectSuffixAttributeWrongClassNameHighlighting(ICSharpIdentifier classDeclaration, string suffix)
        {
            ClassDeclaration = classDeclaration;
            _suffix = suffix;
        }

        public bool IsValid()
        {
            return ClassDeclaration != null && ClassDeclaration.IsValid();
        }

        public string ToolTip { get { return string.Format("Class doesn't have {0} suffix", _suffix); } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}