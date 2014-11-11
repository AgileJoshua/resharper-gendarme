using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.OverrideEqualsMethod
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class OverrideEqualsMethodHighlighting : IHighlighting
    {
        public IClassDeclaration Declaration { get; private set; }

        public OverrideEqualsMethodHighlighting(IClassDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Design: override Equals method."; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}