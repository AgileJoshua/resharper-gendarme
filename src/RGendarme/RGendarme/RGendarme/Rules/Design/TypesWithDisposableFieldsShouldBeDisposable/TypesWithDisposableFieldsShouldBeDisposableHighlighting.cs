using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.TypesWithDisposableFieldsShouldBeDisposable
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class TypesWithDisposableFieldsShouldBeDisposableHighlighting : IHighlighting
    {
        public IClassDeclaration Declaration { get; private set; }

        public TypesWithDisposableFieldsShouldBeDisposableHighlighting(IClassDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Design: types with disposable fields should be disposable."; } }
        public string ErrorStripeToolTip { get; private set; }
        public int NavigationOffsetPatch { get; private set; }
    }
}