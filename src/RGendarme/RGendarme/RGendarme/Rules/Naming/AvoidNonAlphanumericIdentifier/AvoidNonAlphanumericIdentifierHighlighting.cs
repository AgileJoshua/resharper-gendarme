using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Naming.AvoidNonAlphanumericIdentifier
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class AvoidNonAlphanumericIdentifierHighlighting : IHighlighting
    {
        public ICSharpDeclaration Declaration { get; set; }

        public AvoidNonAlphanumericIdentifierHighlighting(ICSharpDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip
        {
            get
            {
                string error = !string.IsNullOrEmpty(Declaration.DeclaredName)
                    ? Declaration.DeclaredName.Replace("_", string.Empty)
                    : string.Empty;

                return string.Format("Naming: avoid on alphanumeric identifier. Use {0} instead.", error);
            }
        }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}