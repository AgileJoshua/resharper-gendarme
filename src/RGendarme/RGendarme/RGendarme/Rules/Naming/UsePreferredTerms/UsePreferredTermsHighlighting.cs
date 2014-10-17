using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Naming.UsePreferredTerms
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class UsePreferredTermsHighlighting : IHighlighting
    {
        public ICSharpDeclaration Declaration { get; private set; }

        private readonly string _wrong;
        private readonly string _rigth;

        public UsePreferredTermsHighlighting(ICSharpDeclaration declaration, string wrong, string right)
        {
            Declaration = declaration;
            _wrong = wrong;
            _rigth = right;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return string.Format("Use '{0}' instead of '{1}'.", _rigth, _wrong); } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}