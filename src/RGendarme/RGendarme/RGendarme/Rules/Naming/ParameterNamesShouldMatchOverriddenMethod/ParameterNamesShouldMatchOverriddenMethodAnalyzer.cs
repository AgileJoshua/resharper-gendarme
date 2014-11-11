using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Naming.ParameterNamesShouldMatchOverriddenMethod
{
    [ElementProblemAnalyzer(new[] { typeof(IMethodDeclaration) }, HighlightingTypes = new[] { typeof(ParameterNamesShouldMatchOverriddenMethodHighlighting) })]
    public class ParameterNamesShouldMatchOverriddenMethodAnalyzer : ElementProblemAnalyzer<IMethodDeclaration>
    {
        protected override void Run(IMethodDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            // TODO UNDONE
//            IReferenceName refName = element.InterfaceQualificationReference;

//            throw new System.NotImplementedException();/
        }
    }

    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class ParameterNamesShouldMatchOverriddenMethodHighlighting : IHighlighting
    {
        public IMethodDeclaration Declaration { get; set; }

        public ParameterNamesShouldMatchOverriddenMethodHighlighting(IMethodDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Naming: parameter names should match overridden method"; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}