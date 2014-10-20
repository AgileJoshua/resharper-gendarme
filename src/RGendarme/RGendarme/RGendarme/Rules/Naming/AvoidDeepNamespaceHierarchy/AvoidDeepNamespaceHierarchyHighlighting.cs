using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Naming.AvoidDeepNamespaceHierarchy
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class AvoidDeepNamespaceHierarchyHighlighting : IHighlighting
    {
        public ICSharpNamespaceDeclaration Declaration { get; private set; }
        private readonly int _maxDeepLevel;
        public AvoidDeepNamespaceHierarchyHighlighting(ICSharpNamespaceDeclaration declaration, int maxDeepLevel)
        {
            Declaration = declaration;
            _maxDeepLevel = maxDeepLevel;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return string.Format("Naming: avoid deep namespace hierarchy. Not more than {0} levels.", _maxDeepLevel); } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}