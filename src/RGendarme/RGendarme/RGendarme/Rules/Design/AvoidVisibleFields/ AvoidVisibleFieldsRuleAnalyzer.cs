using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.AvoidVisibleFields
{
    [ElementProblemAnalyzer(new[] { typeof(IFieldDeclaration) }, HighlightingTypes = new[] { typeof(AvoidVisibleFieldsHighlighting) })]
    public class  AvoidVisibleFieldsAnalyzer : ElementProblemAnalyzer<IFieldDeclaration>
    {
        protected override void Run(IFieldDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            throw new System.NotImplementedException();
        }
    }

    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class AvoidVisibleFieldsHighlighting : IHighlighting
    {
        public IFieldDeclaration FieldDeclaration;
        public AvoidVisibleFieldsHighlighting(IFieldDeclaration fieldDeclaration)
        {
            FieldDeclaration = fieldDeclaration;
        }

        public bool IsValid()
        {
            return FieldDeclaration != null && FieldDeclaration.IsValid();
        }

        public string ToolTip { get { return "Use a property or method instead."; } }
        public string ErrorStripeToolTip { get { return ToolTip; }}
        public int NavigationOffsetPatch { get { return 0; } }
    }
}