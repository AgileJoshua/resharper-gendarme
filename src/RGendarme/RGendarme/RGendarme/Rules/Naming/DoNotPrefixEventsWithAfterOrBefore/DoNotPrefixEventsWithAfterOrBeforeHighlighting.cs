using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Naming.DoNotPrefixEventsWithAfterOrBefore
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class DoNotPrefixEventsWithAfterOrBeforeHighlighting : IHighlighting
    {
        public IEventDeclaration Declaration { get; private set; }
        public DoNotPrefixEventsWithAfterOrBeforeHighlighting(IEventDeclaration declaration)
        {
            Declaration = declaration;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return "Naming: do not prefix events with After or Before. Use a verb in the present and\\or in the past tense"; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}