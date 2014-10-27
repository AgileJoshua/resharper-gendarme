using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.ImplementIComparableCorrectly
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class ImplementIComparableCorrectlyHighlight : IHighlighting
    {
        public IClassDeclaration Declaration { get; private set; }
        private readonly string _details;

        public ImplementIComparableCorrectlyHighlight(IClassDeclaration declaration, string details)
        {
            Declaration = declaration;
            _details = details;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return string.Format("Design: implement IComparable correctly. {0}", _details); } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}