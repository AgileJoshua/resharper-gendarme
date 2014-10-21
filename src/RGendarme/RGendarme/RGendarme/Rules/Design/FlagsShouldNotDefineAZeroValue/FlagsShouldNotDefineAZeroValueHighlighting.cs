using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.FlagsShouldNotDefineAZeroValue
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class FlagsShouldNotDefineAZeroValueHighlighting : IHighlighting
    {
        public IEnumDeclaration Declaration { get; private set; }

        public FlagsShouldNotDefineAZeroValueHighlighting(IEnumDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Design: flags should not define a zero value."; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}