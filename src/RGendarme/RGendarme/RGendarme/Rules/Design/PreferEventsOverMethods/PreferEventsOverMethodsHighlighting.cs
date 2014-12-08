using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.PreferEventsOverMethods
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class PreferEventsOverMethodsHighlighting : IHighlighting
    {
        public PreferEventsOverMethodsHighlighting(IMethodDeclaration declaration)
        {
            Declaration = declaration;
        }

        public IMethodDeclaration Declaration { get; private set; }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Design: This method's name suggests that it could be replaced by an event."; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}