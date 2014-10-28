using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.DisposableTypesShouldHaveFinalizer
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class DisposableTypesShouldHaveFinalizerHighlighting : IHighlighting
    {
        public IClassDeclaration Declaration { get; private set; }

        public DisposableTypesShouldHaveFinalizerHighlighting(IClassDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Design: disposable types with native fields should have finalizer."; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}