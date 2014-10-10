using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Naming.UseCorrectSuffix
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class UseCorrectSuffixHighlighting : IHighlighting
    {
        public  ICSharpIdentifier ClassDeclaration { get; private set; }
        private readonly string _errorMsg;

        public UseCorrectSuffixHighlighting(ICSharpIdentifier classDeclaration, string errorMsg)
        {
            ClassDeclaration = classDeclaration;
            _errorMsg = errorMsg;
        }

        public bool IsValid()
        {
            return ClassDeclaration != null && ClassDeclaration.IsValid();
        }

        public string ToolTip { get { return _errorMsg; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}