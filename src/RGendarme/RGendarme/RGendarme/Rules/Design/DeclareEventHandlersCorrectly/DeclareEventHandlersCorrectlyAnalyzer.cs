using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Impl;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Impl.Types;
using JetBrains.ReSharper.Psi.Resolve.Managed;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using RGendarme.Lib;
using yWorks.yFiles.GraphML.Writer;

namespace RGendarme.Rules.Design.DeclareEventHandlersCorrectly
{
    [ElementProblemAnalyzer(new[] { typeof(IEventDeclaration) }, HighlightingTypes = new[] { typeof(DeclareEventHandlersCorrectlyHighlighting) })]
    public class DeclareEventHandlersCorrectlyAnalyzer : ElementProblemAnalyzer<IEventDeclaration>
    {
        private readonly ISettingsStore _settings;

        public DeclareEventHandlersCorrectlyAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IEventDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (element.DelegateTypeUsage == null) 
                return;

            var delegateType = element.DelegateTypeUsage as IUserDeclaredTypeUsage;
            if (delegateType == null)
                return;

            IReferenceName type = delegateType.TypeName;
            if (type == null)
                return;

            var r = type.Reference.Resolve(); // todo try CurrentResulveResult
            var delegat = r.DeclaredElement as IDelegate;
            if (delegat == null)
                return;

            IMethod method = delegat.InvokeMethod;
            if (!method.ReturnType.IsVoid())
            {
                consumer.AddHighlighting(new DeclareEventHandlersCorrectlyHighlighting(element, "Delegate return type must be void."), element.DelegateName.GetDocumentRange(), element.GetContainingFile());
            }

            if (method.Parameters.Count != 2)
            {
                consumer.AddHighlighting(new DeclareEventHandlersCorrectlyHighlighting(element, "Delegate method must has two parameters"), element.DelegateName.GetDocumentRange(), element.GetContainingFile());
                return;
            }

            IParameter first = method.Parameters[0];
            if (!first.ShortName.Equals("sender"))
            {
                consumer.AddHighlighting(new DeclareEventHandlersCorrectlyHighlighting(element, "First argument must has 'sender' name."), element.DelegateName.GetDocumentRange(), element.GetContainingFile());
            }

            IParameter second = method.Parameters[1];
            IExpressionType t = second.Type;

            IDeclaredType sysType = TypeFactory.CreateTypeByCLRName("System.EventArgs", element.GetPsiModule(), element.GetResolveContext());
            if (!t.IsImplicitlyConvertibleTo(sysType, ClrPredefinedTypeConversionRule.INSTANCE))
            {
                consumer.AddHighlighting(new DeclareEventHandlersCorrectlyHighlighting(element, "Second argument must be 'System.EventArgs' or extend it."), element.DelegateName.GetDocumentRange(), element.GetContainingFile());
            }
        }
    }

    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class DeclareEventHandlersCorrectlyHighlighting : IHighlighting
    {
        public IEventDeclaration Declaration { get; private set; }
        private string _details;

        public DeclareEventHandlersCorrectlyHighlighting(IEventDeclaration declaration, string details)
        {
            Declaration = declaration;
            _details = details;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return string.Format("Design: declare event handlers correctly. {0}", _details); } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}