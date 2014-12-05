using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;

namespace RGendarme.Rules.Design.MarkAssemblyWith
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class MarkAssemblyWithHighlighting : IHighlighting
    {
        private readonly string _attributeName;

        public MarkAssemblyWithHighlighting(string attributeName)
        {
            _attributeName = attributeName;
        }

        public bool IsValid()
        {
            return true;
        }

        public string ToolTip { get { return string.Format("Design: mark assembly with '{0}' attribute.", _attributeName); } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}