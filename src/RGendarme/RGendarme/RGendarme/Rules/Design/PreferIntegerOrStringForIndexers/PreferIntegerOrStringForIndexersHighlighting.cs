using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.PreferIntegerOrStringForIndexers
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class PreferIntegerOrStringForIndexersHighlighting : IHighlighting
    {
        public IIndexerDeclaration Declaration { get; private set; }
        public PreferIntegerOrStringForIndexersHighlighting(IIndexerDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Use only int, long or string in indexer."; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; }}
    }
}