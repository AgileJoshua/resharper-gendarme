using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Naming.UseCorrectSuffix.Attribute.NotImplementType
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(UseCorrectSuffixAttributeNotImplementAttributeHighlighting) })]
    public class UseCorrectSuffixAttributeNotImplementAttributeAnalyzer : ElementProblemAnalyzer<IClassDeclaration>
    {
        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            // 1. get class name
            // 2. get exntend list
            // 3. check if class has Attribute suffix but doesn't extend Attribute class

            throw new System.NotImplementedException();
        }
    }

    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class UseCorrectSuffixAttributeNotImplementAttributeHighlighting : IHighlighting
    {
        public IClassDeclaration ClassDeclaration { get; private set; }
        public UseCorrectSuffixAttributeNotImplementAttributeHighlighting(IClassDeclaration classDeclaration)
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