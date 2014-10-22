using System.Collections.Generic;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Design.EnsureSymmetryForOverloadedOperators
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(EnsureSymmetryForOverloadedOperatorsHighlighting) })]
    public class EnsureSymmetryForOverloadedOperatorsAnalyzer : ElementProblemAnalyzer<IClassDeclaration>
    {
        private readonly IDictionary<string, string> _rules;
        private readonly ISettingsStore _settings;
//
        public EnsureSymmetryForOverloadedOperatorsAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
            _rules = new Dictionary<string, string>
            {
                {"-", "+"},
                {"+", "-"},

                {"*", "/"},
                {"/", "*"},

//                {"/", "%"},
//                {"%", "/"},

                {">", "<"},
                {"<", ">"},

                {">=", "<="},
                {"<=", ">="},

                {"==", "!="},
                {"!=", "=="},

                {"true", "false"},
                {"false", "true"}
            };
        }

        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (element.Body == null || element.Body.Operators.Count == 0)
                return;

            var dict = new Dictionary<string, IOperatorDeclaration>();
            foreach (IOperatorDeclaration op in element.Body.OperatorsEnumerable)
            {
                if (!dict.ContainsKey(op.DeclaredName))
                    dict.Add(op.DeclaredName, op);
            }

            foreach (var item in dict)
            {
                if (!_rules.ContainsKey(item.Key)) continue;

                string mustImplement = _rules[item.Key];
                if (!dict.ContainsKey(mustImplement))
                {
                    consumer.AddHighlighting(new EnsureSymmetryForOverloadedOperatorsHighlighting(element, mustImplement), item.Value.OperatorKeyword.GetDocumentRange(), item.Value.GetContainingFile());
                }
            }
        }
    }
}