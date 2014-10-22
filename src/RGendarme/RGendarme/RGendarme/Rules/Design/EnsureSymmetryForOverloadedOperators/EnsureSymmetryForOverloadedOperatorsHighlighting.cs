using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.EnsureSymmetryForOverloadedOperators
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class EnsureSymmetryForOverloadedOperatorsHighlighting : IHighlighting
    {
        public IClassDeclaration Declaration { get; private set; }

        private readonly string _opName;

        public EnsureSymmetryForOverloadedOperatorsHighlighting(IClassDeclaration declaration, string opName)
        {
            Declaration = declaration;
            _opName = opName;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return string.Format("Design: you also need to implement '{0}' operator.", _opName); } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}