using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Rules.Naming.AvoidTypeInterfaceInconsistency
{
    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class AvoidTypeInterfaceInconsistencyHighlighting : IHighlighting
    {
        public IClassDeclaration ClassDeclaration { get; private set; }
        public IInterfaceDeclaration InterfaceDeclaration { get; private set; }

        public AvoidTypeInterfaceInconsistencyHighlighting([NotNull]IClassDeclaration classDeclaration, [NotNull]IInterfaceDeclaration interfaceDeclaration)
        {
            ClassDeclaration = classDeclaration;
            InterfaceDeclaration = interfaceDeclaration;
        }

        public bool IsValid()
        {
            return ClassDeclaration != null && ClassDeclaration.IsValid();
        }

        public string ToolTip
        {
            get
            {
                return string.Format("Naming: avoid type interface inconsistency. You need implement '{0}' interface.", InterfaceDeclaration.DeclaredName);
            }
        }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}