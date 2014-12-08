using System;
using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Design.OperatorEqualsShouldBeOverloaded
{
    [ElementProblemAnalyzer(new[] { typeof(ICSharpTypeDeclaration) }, HighlightingTypes = new[] { typeof(OperatorEqualsShouldBeOverloadedHighlighting) })]
    public class OperatorEqualsShouldBeOverloadedAnalyzer : ElementProblemAnalyzer<ICSharpTypeDeclaration>
    {
        private readonly ISettingsStore _settings;

        public OperatorEqualsShouldBeOverloadedAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(ICSharpTypeDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var cls = element as IClassDeclaration;
            if (cls != null && cls.NameIdentifier != null && !cls.OperatorDeclarations.IsEmpty)
            {
                bool hasAdditionOp = HasOperator(cls.OperatorDeclarations, "op_Addition") || HasOperator(cls.OperatorDeclarations, "op_UnaryPlus");
                bool hasSubtractionOp = HasOperator(cls.OperatorDeclarations, "op_Subtraction") || HasOperator(cls.OperatorDeclarations, "op_UnaryNegation");
                bool hasEqualityOp = HasOperator(cls.OperatorDeclarations, "op_Equality");

                if (hasAdditionOp && hasSubtractionOp && !hasEqualityOp)
                    consumer.AddHighlighting(new OperatorEqualsShouldBeOverloadedHighlighting(element), cls.NameIdentifier.GetDocumentRange(), cls.GetContainingFile());
            }

            var strct = element as IStructDeclaration;
            if (strct != null && strct.NameIdentifier != null && !strct.MemberDeclarations.IsEmpty)
            {
                // todo move this to simple extsnsion method
                bool hasEqual = strct.MethodDeclarations.Any(method => !string.IsNullOrEmpty(method.DeclaredName)
                    && method.DeclaredName.Equals("Equals")
                    && method.ModifiersList != null
                    && method.ModifiersList.HasModifier(CSharpTokenType.OVERRIDE_KEYWORD)
                    && method.ParameterDeclarations.Count == 1
                    && string.Compare(method.ParameterDeclarations[0].Type.ToString(), "System.Object",
                        StringComparison.Ordinal) == 0);

                bool hasEqualityOp = HasOperator(strct.OperatorDeclarations, "op_Equality");

                if (hasEqual && !hasEqualityOp)
                    consumer.AddHighlighting(new OperatorEqualsShouldBeOverloadedHighlighting(element), strct.NameIdentifier.GetDocumentRange(), strct.GetContainingFile());
            }
        }

#warning todo refactor it as extension method for reuse
        private bool HasOperator([NotNull]TreeNodeCollection<IOperatorDeclaration> cls, [NotNull]string operatorName)
        {
            return cls.Any(
                op => op.DeclaredElement != null
                      && !string.IsNullOrEmpty(op.DeclaredElement.ShortName)
                      && op.DeclaredElement.ShortName.Equals(operatorName));
        }
    }
}