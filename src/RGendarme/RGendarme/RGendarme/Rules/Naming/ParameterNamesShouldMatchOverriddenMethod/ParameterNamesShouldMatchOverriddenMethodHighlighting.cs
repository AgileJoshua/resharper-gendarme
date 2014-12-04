using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Naming.ParameterNamesShouldMatchOverriddenMethod
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class ParameterNamesShouldMatchOverriddenMethodHighlighting : IHighlighting
    {
        public ICSharpParameterDeclaration Declaration { get; set; }

        private readonly string _rightParamName;

        public ParameterNamesShouldMatchOverriddenMethodHighlighting(ICSharpParameterDeclaration declaration, string rightParamName)
        {
            Declaration = declaration;
            _rightParamName = rightParamName;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return string.Format("Naming: parameter names should match overridden method. Use '{0}' instead.", _rightParamName); } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}