using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Design.ConsiderAddingInterface
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class ConsiderAddingInterfaceHighlighting : IHighlighting
    {
        public IInterfaceDeclaration Declaration { get; private set; }

        private readonly string _interfaceName;

        public ConsiderAddingInterfaceHighlighting(IInterfaceDeclaration declaration, string interfaceName)
        {
            Declaration = declaration;
            _interfaceName = interfaceName;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return string.Format("Design: consider adding {0} interface.", _interfaceName); } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}