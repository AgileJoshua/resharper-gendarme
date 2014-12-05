using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;

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
                ThrowWarning(element, consumer, "Delegate return type must be void.");
            }

            if (method.Parameters.Count != 2)
            {
                ThrowWarning(element, consumer, "Delegate method must has two parameters");
                return;
            }

            IParameter first = method.Parameters[0];
            if (!first.ShortName.Equals("sender"))
            {
                ThrowWarning(element, consumer, "First argument must has 'sender' name.");
            }

#warning do not use magic string, use typeof(System.EventArgs).FullName instead.
            IDeclaredType sysType = TypeFactory.CreateTypeByCLRName("System.EventArgs", element.GetPsiModule(), element.GetResolveContext());

            IParameter second = method.Parameters[1];

            if (second.Type.IsOpenType) // is generic method
            {   
                // todo rafactor this code block 
                // look at this later
//                string secondTypeShortName = second.Type.ToString();
//                foreach (ITypeParameter p in delegat.TypeParameters)
//                {
//                    if (p.ShortName.Equals(secondTypeShortName))
//                    {
//                        IExpressionType t = p.EffectiveBaseClass();
//                        IExpressionType t2 = p.Type();
//                    }
//                }
//                foreach (var a in args.TypeArgumentNodes)
//                {
//                    int i = 1;
//                }

                // todo is'e ugly implementation but it's working. Refactot it later.
                bool isExtendEventArgs = false;
                ITypeArgumentList args = type.TypeArgumentList;
                if (args != null)
                {
                    foreach (IType a in args.TypeArguments)
                    {
                        if (a.IsImplicitlyConvertibleTo(sysType, ClrPredefinedTypeConversionRule.INSTANCE))
                        {
                            isExtendEventArgs = true;
                        }
                    }
                }

                if (!isExtendEventArgs)
                {
                    ThrowWarning(element, consumer, "Second argument must be 'System.EventArgs' or extend it.");
                }

            }
            else
            {
                if (!second.Type.IsImplicitlyConvertibleTo(sysType, ClrPredefinedTypeConversionRule.INSTANCE))
                {
                    ThrowWarning(element, consumer, "Second argument must be 'System.EventArgs' or extend it.");    
                }
            }
        }

        private void ThrowWarning(IEventDeclaration element, IHighlightingConsumer consumer, string message)
        {
            consumer.AddHighlighting(new DeclareEventHandlersCorrectlyHighlighting(element,message), element.DelegateName.GetDocumentRange(), element.GetContainingFile());
        }
    }
}