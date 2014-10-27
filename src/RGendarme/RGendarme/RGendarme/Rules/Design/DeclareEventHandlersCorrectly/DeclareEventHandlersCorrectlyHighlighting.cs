using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.DeclareEventHandlersCorrectly
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class DeclareEventHandlersCorrectlyHighlighting : IHighlighting
    {
        public IEventDeclaration Declaration { get; private set; }
        private string _details;

        public DeclareEventHandlersCorrectlyHighlighting(IEventDeclaration declaration, string details)
        {
            Declaration = declaration;
            _details = details;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return string.Format("Design: declare event handlers correctly. {0}", _details); } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}