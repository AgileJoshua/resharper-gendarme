using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.ProvideAlternativeNamesForOperatorOverloads
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class ProvideAlternativeNamesForOperatorOverloadsHighlighting : IHighlighting
    {
        public ProvideAlternativeNamesForOperatorOverloadsHighlighting(IOperatorDeclaration declaration, string methodName)
        {
            Declaration = declaration;
            _methodName = methodName;
        }

        private readonly string _methodName;
        public IOperatorDeclaration Declaration { get; private set; }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return string.Format("Design: provide public method with {0} name.", _methodName); } }
        public string ErrorStripeToolTip { get; private set; }
        public int NavigationOffsetPatch { get; private set; }
    }
}