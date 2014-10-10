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
            // TODO: NotImplemented
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
                        if (extendTypeName.Equals("Attribute"))
                            isExtendAttribute = true;
                    }
                }
            }

            if (!isExtendAttribute) 
                return;

            // 3. get class name
            string name = element.NameIdentifier.Name;
            if (!string.IsNullOrEmpty(name) && !name.EndsWith("Attribute"))
            {
                ICSharpIdentifier identifier = element.NameIdentifier;
                consumer.AddHighlighting(new UseCorrectSuffixAttributeWrongClassNameHighlighting(identifier), identifier.GetDocumentRange(), identifier.GetContainingFile());
            }
        }
    }

    // todo also add use case when class has Attribute suffix but doesn't extend Attribute class
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class UseCorrectSuffixAttributeWrongClassNameHighlighting : IHighlighting
    {
        public ICSharpIdentifier ClassDeclaration { get; private set; }
        public UseCorrectSuffixAttributeWrongClassNameHighlighting(ICSharpIdentifier classDeclaration)
        {
            ClassDeclaration = classDeclaration;
        }

        public bool IsValid()
        {
            return ClassDeclaration != null && ClassDeclaration.IsValid();
        }

        public string ToolTip { get { return "Class doesn't have Attbite suffix"; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}