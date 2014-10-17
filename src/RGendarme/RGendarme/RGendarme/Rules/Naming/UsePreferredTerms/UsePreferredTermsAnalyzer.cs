﻿using System.Collections.Generic;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Naming.UsePreferredTerms
{
    [ElementProblemAnalyzer(new[] { typeof(ICSharpDeclaration) }, HighlightingTypes = new[] { typeof(UsePreferredTermsHighlighting) })]
    public class UsePreferredTermsAnalyzer : ElementProblemAnalyzer<ICSharpDeclaration>
    {
        private readonly IDictionary<string, string> _rules;

        public UsePreferredTermsAnalyzer()
        {
            _rules = new Dictionary<string, string>
            {
                {"Arent", "AreNot"},
                {"Cancelled", "Canceled"},
                {"Cant", "Cannot"},
                {"ComPlus", "EnterpriseServices"},
                {"Couldnt", "CouldNot"},
                {"Didnt", "DidNot"},
                {"Doesnt", "DoesNot"},
                {"Dont", "DoNot"},
                {"Hadnt", "HadNot"},
                {"Hasnt", "HasNot"},
                {"Havent", "HaveNot"},
                {"Indices", "Indexes"},
                {"Isnt", "IsNot"},
                {"LogIn", "LogOn"},
                {"LogOut", "LogOff"},
                {"Shouldnt", "ShouldNot"},
                {"SignOn", "SignIn"},
                {"SignOff", "SignOut"},
                {"Wasnt", "WasNot"},
                {"Werent", "WereNot"},
                {"Wont", "WillNot"},
                {"Wouldnt", "WouldNot"},
                {"Writeable", "Writable"}
            };
        }

        protected override void Run(ICSharpDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (element.NameIdentifier == null) return;

            string name = element.NameIdentifier.Name;
            if (string.IsNullOrEmpty(name)) return;

            foreach (var rule in _rules)
            {
                if (name.Contains(rule.Key))
                {
                    consumer.AddHighlighting(new UsePreferredTermsHighlighting(element, rule.Key, rule.Value), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
                }
            }
        }
    }
}