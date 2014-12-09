using System;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Design.OverrideEqualsMethod
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(OverrideEqualsMethodHighlighting) })]
    public class OverrideEqualsMethodAnalyzer : ElementProblemAnalyzer<IClassDeclaration>
    {
        private readonly ISettingsStore _settings;

        public OverrideEqualsMethodAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (element.Body == null || element.Body.Operators.Count == 0)
                return;

            // 1. Class must implement == operator
            bool isImplEqulaityOperator = false;
            foreach (IOperatorDeclaration op in element.Body.OperatorsEnumerable)
            {
                if (!string.IsNullOrEmpty(op.DeclaredName) && op.DeclaredName == "==")
                {
                    isImplEqulaityOperator = true;
                    break;
                }
            }

            if (!isImplEqulaityOperator)
                return;

            // 2. Class do not implemnet Equals method
#warning remove System.Object magic string, use typeof(object).FullName instead, Use IsImplicitlyConvertibleTo
            bool isImplEqualsMethod = false;
            foreach (IMethodDeclaration method in element.MethodDeclarationsEnumerable)
            {
                if (!string.IsNullOrEmpty(method.DeclaredName)
                    && method.DeclaredName.Equals("Equals")
                    && method.ModifiersList != null
                    && method.ModifiersList.HasModifier(CSharpTokenType.OVERRIDE_KEYWORD)
                    && method.ParameterDeclarations.Count == 1
                    && string.Compare(method.ParameterDeclarations[0].Type.ToString(), "System.Object",
                        StringComparison.Ordinal) == 0)
                {
                    isImplEqualsMethod = true;
                    break;
                }
            }

            if (!isImplEqualsMethod)
            {
                consumer.AddHighlighting(new OverrideEqualsMethodHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }
        }
    }
}