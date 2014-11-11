using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.MainShouldNotBePublic
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class MainShouldNotBePublicHighlighting : IHighlighting
    {
        public IMethodDeclaration Declaration { get; set; }
        public MainShouldNotBePublicHighlighting(IMethodDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Design: Main should not be public."; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}