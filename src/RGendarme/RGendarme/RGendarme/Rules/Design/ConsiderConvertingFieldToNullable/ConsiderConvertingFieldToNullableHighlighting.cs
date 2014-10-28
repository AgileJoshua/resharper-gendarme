using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.ConsiderConvertingFieldToNullable
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class ConsiderConvertingFieldToNullableHighlighting : IHighlighting
    {
        public IFieldDeclaration Declaration { get; private set; }
        private readonly string _warningMessage;

        public ConsiderConvertingFieldToNullableHighlighting(IFieldDeclaration declaration, string warningMessage)
        {
            Declaration = declaration;
            _warningMessage = warningMessage;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return string.Format("Design: consider converting field to nullable. {0}", _warningMessage); } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}