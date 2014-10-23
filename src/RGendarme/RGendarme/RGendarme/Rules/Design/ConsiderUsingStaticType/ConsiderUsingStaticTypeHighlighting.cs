using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.ConsiderUsingStaticType
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class ConsiderUsingStaticTypeHighlighting : IHighlighting
    {
        public IClassDeclaration Declaration { get; private set; }

        public ConsiderUsingStaticTypeHighlighting(IClassDeclaration declaration)
        {
            Declaration = declaration;
        }
        
        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Design: consider using static type."; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}