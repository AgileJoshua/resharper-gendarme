using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.PreferXmlAbstractions
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class PreferXmlAbstractionshlighting : IHighlighting
    {
        public ICSharpDeclaration Declaration { get; private set; }

        public PreferXmlAbstractionshlighting(ICSharpDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Design: prefer xml abstractions. Instead use abstract types like IXPathNavigable, XmlReader, XmlWriter, or subtypes of XmlNode"; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}