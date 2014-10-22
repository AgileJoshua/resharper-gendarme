using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Custom.SameKeyInDictionaryDeclaration
{
    [StaticSeverityHighlighting(Severity.ERROR, CSharpLanguage.Name)]
    public class SameKeyInDictionaryDeclarationHighlight : IHighlighting
    {
        public ILocalVariableDeclaration Statement { get; private set; }
        public SameKeyInDictionaryDeclarationHighlight(ILocalVariableDeclaration statement)
        {
            Statement = statement;
        }

        public bool IsValid()
        {
            return Statement != null && Statement.IsValid();
        }

        public string ToolTip { get { return "Custom: duplicate key declaration."; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}