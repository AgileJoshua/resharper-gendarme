using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Naming.UsePropertyInsteadMethodWithGetPrefix
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class UsePropertyInsteadMethodWithGetPrefixHighlighting : IHighlighting
    {
        public IMethodDeclaration Declaration { get; private set; }

        public UsePropertyInsteadMethodWithGetPrefixHighlighting(IMethodDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Naming: Use property instead."; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}