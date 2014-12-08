using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Naming.AvoidRedundancyInTypeName
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class AvoidRedundancyInTypeNameHighlighting : IHighlighting
    {
        public ICSharpTypeDeclaration Declaration { get; private set; }

        private readonly string _errorMessage;

        public AvoidRedundancyInTypeNameHighlighting(ICSharpTypeDeclaration declaration, string errorMessage)
        {
            Declaration = declaration;
            _errorMessage = errorMessage;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return string.Format("Naming: avoid redundancy in type name. Use '{0}' instead.", _errorMessage); } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}