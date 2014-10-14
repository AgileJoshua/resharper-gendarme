using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.AvoidMultidimensionalIndexer
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class AvoidMultidimensionalIndexerHighlighting : IHighlighting
    {
        public IIndexerDeclaration Declaration { get; private set; }
    
        public AvoidMultidimensionalIndexerHighlighting(IIndexerDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Avoid multidimensional indexer. Use method instead."; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}