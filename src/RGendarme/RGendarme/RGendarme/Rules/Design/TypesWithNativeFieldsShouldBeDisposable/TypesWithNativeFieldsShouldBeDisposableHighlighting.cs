using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.TypesWithNativeFieldsShouldBeDisposable
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class TypesWithNativeFieldsShouldBeDisposableHighlighting : IHighlighting
    {
        public IClassDeclaration Declaration { get; private set; }

        public TypesWithNativeFieldsShouldBeDisposableHighlighting(IClassDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Design: types with native fields should be disposable."; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}