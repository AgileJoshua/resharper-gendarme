using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.TypesShouldBeInsideNamespaces
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class TypesShouldBeInsideNamespacesHighlighting : IHighlighting
    {
        public ICSharpTypeDeclaration Declaration { get; private set; }

        public TypesShouldBeInsideNamespacesHighlighting(ICSharpTypeDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Design: types should be inside namespaces."; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}