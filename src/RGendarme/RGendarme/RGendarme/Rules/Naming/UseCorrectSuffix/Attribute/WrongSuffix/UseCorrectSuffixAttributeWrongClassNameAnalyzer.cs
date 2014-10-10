using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Naming.UseCorrectSuffix.Attribute.WrongSuffix
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(UseCorrectSuffixAttributeWrongClassNameHighlighting) })]
    public class UseCorrectSuffixAttributeWrongClassNameAnalyzer : ElementProblemAnalyzer<IClassDeclaration>
    {
        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            // TODO: NotImplemented
            // 1. get class name
            // 2. get exntend list
            // 3. check if class extends Attribute class, it has to have Attbite suffix

            //throw new System.NotImplementedException();
        }
    }

    // todo also add use case when class has Attribute suffix but doesn't extend Attribute class
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class UseCorrectSuffixAttributeWrongClassNameHighlighting : IHighlighting
    {
        public IClassDeclaration ClassDeclaration { get; private set; }
        public UseCorrectSuffixAttributeWrongClassNameHighlighting(IClassDeclaration classDeclaration)
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