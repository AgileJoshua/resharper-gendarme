using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.MissingAttributeUsageOnCustomAttribute
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class MissingAttributeUsageOnCustomAttributeHighlighting : IHighlighting
    {
        public IClassDeclaration Declaration { get; private set; }

        public MissingAttributeUsageOnCustomAttributeHighlighting(IClassDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Design: missing AttributeUsage on custom attribute."; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; }}
    }
}