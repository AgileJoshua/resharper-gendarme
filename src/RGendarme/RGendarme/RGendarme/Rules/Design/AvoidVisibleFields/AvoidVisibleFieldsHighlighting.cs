using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.AvoidVisibleFields
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class AvoidVisibleFieldsHighlighting : IHighlighting
    {
        public IMultipleFieldDeclaration FieldDeclaration;
        public AvoidVisibleFieldsHighlighting(IMultipleFieldDeclaration fieldDeclaration)
        {
            FieldDeclaration = fieldDeclaration;
        }

        public bool IsValid()
        {
            return FieldDeclaration != null && FieldDeclaration.IsValid();
        }

        public string ToolTip { get { return "Public field. Use a property or method instead."; } }
        public string ErrorStripeToolTip { get { return ToolTip; }}
        public int NavigationOffsetPatch { get { return 0; } }
    }
}