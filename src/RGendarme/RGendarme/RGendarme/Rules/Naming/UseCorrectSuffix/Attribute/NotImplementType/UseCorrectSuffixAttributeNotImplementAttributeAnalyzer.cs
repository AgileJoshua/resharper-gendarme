using System;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Naming.UseCorrectSuffix.Attribute.NotImplementType
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(UseCorrectSuffixAttributeNotImplementAttributeHighlighting) })]
    public class UseCorrectSuffixAttributeNotImplementAttributeAnalyzer : ElementProblemAnalyzer<IClassDeclaration>
    {
        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            Analyze(element, consumer, "Attribute", (id, suffix) => new UseCorrectSuffixAttributeNotImplementAttributeHighlighting(id, suffix));
        }

        private void Analyze(IClassDeclaration element, IHighlightingConsumer consumer, string suffix,
            Func<ICSharpIdentifier, string, IHighlighting> highlighting)
        {
            // 1. get class name
            string name = element.NameIdentifier.Name;

            if (!string.IsNullOrEmpty(name) && !name.EndsWith(suffix))
                return;

            // 2. get exntend list
            IExtendsList extends = element.ExtendsList;
            if (extends == null)
            {
                ICSharpIdentifier nameIdentifier = element.NameIdentifier;
                consumer.AddHighlighting(highlighting(nameIdentifier, suffix), nameIdentifier.GetDocumentRange(), nameIdentifier.GetContainingFile());
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
                consumer.AddHighlighting(highlighting(nameIdentifier, suffix), nameIdentifier.GetDocumentRange(), nameIdentifier.GetContainingFile());
            }
        }
    }

    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class UseCorrectSuffixAttributeNotImplementAttributeHighlighting : IHighlighting
    {
        public ICSharpIdentifier ClassDeclaration { get; private set; }
        private readonly string _suffix;

        public UseCorrectSuffixAttributeNotImplementAttributeHighlighting(ICSharpIdentifier classDeclaration, string suffix)
        {
            ClassDeclaration = classDeclaration;
            _suffix = suffix;
        }

        public bool IsValid()
        {
            return ClassDeclaration != null && ClassDeclaration.IsValid();
        }

        public string ToolTip { get { return string.Format("Has {0} suffix but doesn't extend {0} class", _suffix); } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; }}
    }
}