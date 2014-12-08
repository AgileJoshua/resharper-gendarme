using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.InternalNamespacesShouldNotExposeTypes
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class InternalNamespacesShouldNotExposeTypesHighlighting : IHighlighting
    {
        public InternalNamespacesShouldNotExposeTypesHighlighting(ICSharpTypeDeclaration declaration)
        {
            Declaration = declaration;
        }

        public ICSharpTypeDeclaration Declaration { get; private set; }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Design: internal namespaces should not expose types. Make class not public."; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}