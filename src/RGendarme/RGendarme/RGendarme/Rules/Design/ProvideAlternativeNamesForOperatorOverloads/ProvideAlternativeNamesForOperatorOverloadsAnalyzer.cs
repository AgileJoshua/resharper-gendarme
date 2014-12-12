using System;
using System.Collections.Generic;
using JetBrains.Application.DataContext;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib;
using RGendarme.Lib.Extenstions;
using RGendarme.Settings.Design;

namespace RGendarme.Rules.Design.ProvideAlternativeNamesForOperatorOverloads
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(ProvideAlternativeNamesForOperatorOverloadsHighlighting) })]
    public class ProvideAlternativeNamesForOperatorOverloadsAnalyzer : ElementProblemAnalyzer<IClassDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        // todo: refactor it later - remove magic strings
        private readonly IDictionary<string, string> _rules; 

        public ProvideAlternativeNamesForOperatorOverloadsAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
            // todo later use this without magic strings
            // todo use MethodSignature (see in orginial Gendarme project)
//            _rules = new Dictionary<ITokenNodeType, string>
//            {
//                {CSharpTokenType.PLUS, "Add"},
//                {CSharpTokenType.MINUS, ""},
//                
//            };

            _rules = new Dictionary<string, string>
            {
                // unary
                {"op_UnaryPlus", "Plus"},
                {"op_UnaryNegation", "Negate"},
                {"op_LogicalNot", "LogicalNot"},
                {"op_OnesComplement", "OnesComplement"},

                {"op_Increment", "Increment"},
                {"op_Decrement", "Decrement"},
                {"op_True", "IsTrue"},
                {"op_False", "IsFalse"},

                // binary
                {"op_Addition", "Add"},
                {"op_Subtraction", "Subtract"},
                {"op_Multiply", "Multiply"},
                {"op_Division", "Divide"},
                {"op_Modulus", "Modulus"},

                {"op_BitwiseAnd", "BitwiseAnd"},
                {"op_BitwiseOr", "BitwiseOr"},
                {"op_ExclusiveOr", "ExclusiveOr"},

                {"op_LeftShift", "LeftShift"},
                {"op_RightShift", "RightShift"},

                {"op_Inequality", "Equals"},
                {"op_GreaterThan", "Compare"},
                {"op_LessThan", "Compare"},
                {"op_GreaterThanOrEqual", "Compare"},
                {"op_LessThanOrEqual", "Compare"}
            };
        }

        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            if (element.NameIdentifier == null || element.OperatorDeclarations.IsEmpty)
                return;

            foreach (IOperatorDeclaration op in element.OperatorDeclarationsEnumerable)
            {
                if (op.DeclaredElement == null || string.IsNullOrEmpty(op.DeclaredElement.ShortName))
                    return;

                string name = op.DeclaredElement.ShortName;

                if (_rules.ContainsKey(name))
                {
                    string methodName = _rules[name];

                    bool isMethodExists = element.MemberDeclarations.Any(
                        m => !string.IsNullOrEmpty(m.DeclaredName)
                             && m.DeclaredName.Equals(methodName)
                             && m.IsPublic());

                    if (!isMethodExists)
                    {
                        consumer.AddHighlighting(new ProvideAlternativeNamesForOperatorOverloadsHighlighting(op, methodName), op.OperatorKeyword.GetDocumentRange(), op.GetContainingFile());
                    }
                }
            }
            
            // throw new System.NotImplementedException();
        }

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<DesignRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.ProvideAlternativeNamesForOperatorOverloadsEnabled;
        }
    }
}