using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.EnumsShouldUseInt32
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class EnumsShouldUseInt32Highlighting : IHighlighting
    {
        public EnumsShouldUseInt32Highlighting(IEnumDeclaration declaration)
        {
            Declaration = declaration;
        }

        public IEnumDeclaration Declaration { get; private set; }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Design: enums should use CLS-compliant integral types: byte, short, int and long"; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}