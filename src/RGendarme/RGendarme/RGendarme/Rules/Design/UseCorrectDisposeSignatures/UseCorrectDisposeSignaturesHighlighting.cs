using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.UseCorrectDisposeSignatures
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class UseCorrectDisposeSignaturesHighlighting : IHighlighting
    {
        public IClassDeclaration Declaration { get; private set; }

        private readonly string _detailInfo;

        public UseCorrectDisposeSignaturesHighlighting(IClassDeclaration declaration, string detailErrorMessage)
        {
            Declaration = declaration;
            _detailInfo = detailErrorMessage;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return string.Format("Design: use correct dispose dignatures. {0}", _detailInfo); } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}