using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib;

namespace RGendarme.Rules.Design.FlagsShouldNotDefineAZeroValue
{
    [ElementProblemAnalyzer(new[] { typeof(IEnumDeclaration) }, HighlightingTypes = new[] { typeof(FlagsShouldNotDefineAZeroValueHighlighting) })]
    public class FlagsShouldNotDefineAZeroValueAnalyzer : ElementProblemAnalyzer<IEnumDeclaration>
    {
        private readonly ISettingsStore _settings;

        public FlagsShouldNotDefineAZeroValueAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IEnumDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            // 1. check for Flags attribute
            if (!AnalyzerHelper.HasAttribute(element, "System.FlagsAttribute"))
                return;

            // 2. check for zero value
            IEnumMemberDeclaration zeroMember = null;
            foreach (IEnumMemberDeclaration item in element.EnumMemberDeclarationsEnumerable)
            {
                var expr = item.ValueExpression as ICSharpLiteralExpression;
                if (expr == null) continue;

                ConstantValue val = expr.ConstantValue;
                if (val.IsZero())
                {
                    zeroMember = item;
                    break;
                }
            }

            if (zeroMember != null)
            {
                consumer.AddHighlighting(new FlagsShouldNotDefineAZeroValueHighlighting(element), zeroMember.GetDocumentRange(), element.GetContainingFile());
            }
        }
    }
}