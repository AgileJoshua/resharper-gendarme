using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using RGendarme.Lib;

namespace RGendarme.Rules.Design.ImplementEqualsAndGetHashCodeInPair
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class ImplementEqualsAndGetHashCodeInPairHighlighting : IHighlighting
    {
        public IMethodDeclaration Declaration { get; private set; }

        public ImplementEqualsAndGetHashCodeInPairHighlighting(IMethodDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return string.Format("Design: You need implement {0} method too.", CommonMethodName.EQUALS); } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}