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
            // 1. get class name
            string name = element.NameIdentifier.Name;

            if (!string.IsNullOrEmpty(name) && !name.EndsWith("Attribute"))
                return;
            
            // 2. get exntend list
            IExtendsList extends = element.ExtendsList;
            if (extends == null)
            {
                ICSharpIdentifier nameIdentifier = element.NameIdentifier;

                consumer.AddHighlighting(new UseCorrectSuffixAttributeNotImplementAttributeHighlighting(nameIdentifier), nameIdentifier.GetDocumentRange(), nameIdentifier.GetContainingFile());
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
                        if (extendTypeName.Equals("Attribute"))
                            isImplementAttribute = true;
                    }
                }
            }

            if (!isImplementAttribute)
            {
                ICSharpIdentifier nameIdentifier = element.NameIdentifier;
                consumer.AddHighlighting(new UseCorrectSuffixAttributeNotImplementAttributeHighlighting(nameIdentifier), nameIdentifier.GetDocumentRange(), nameIdentifier.GetContainingFile());
            }
        }
    }

    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class UseCorrectSuffixAttributeNotImplementAttributeHighlighting : IHighlighting
    {
        public ICSharpIdentifier ClassDeclaration { get; private set; }
        public UseCorrectSuffixAttributeNotImplementAttributeHighlighting(ICSharpIdentifier classDeclaration)
        {
            ClassDeclaration = classDeclaration;
        }

        public bool IsValid()
        {
            return ClassDeclaration != null && ClassDeclaration.IsValid();
        }

        public string ToolTip { get { return "Has Attribute suffix but doesn't extend Attribute class"; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; }}
    }
}