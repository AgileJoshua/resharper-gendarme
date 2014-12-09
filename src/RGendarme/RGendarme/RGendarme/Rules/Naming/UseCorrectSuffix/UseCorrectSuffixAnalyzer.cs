using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Application.DataContext;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib;
using RGendarme.Settings.Naming;

namespace RGendarme.Rules.Naming.UseCorrectSuffix
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(UseCorrectSuffixHighlighting) })]
    public class UseCorrectSuffixAnalyzer : ElementProblemAnalyzer<IClassDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        public UseCorrectSuffixAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            // key - name, value - base type
            // todo do not use magic strings
            var dict = new Dictionary<string, string>
            {
                {"Attribute", "System.Attribute"},
                {"EventArgs", "System.EventArgs"},
                {"Exception", "System.Exception"},
                {"DataSet", "System.Data.DataSet"},
                {"Stream", "System.IO.Stream"},
                {"Permission", "System.Security.IPermission"},
                {"Condition", "System.Security.Policy.IMembershipCondition"},
                {"Collection|Queue", "System.Collections.Queue"},
                {"Collection|DataTable", "System.Data.DataTable"},
                {"Collection|Stack", "System.Collections.Stack"}
            };

            foreach (var kvp in dict)
            {
                Analyze(element, consumer, kvp.Key, kvp.Value, (id, suffix) => new UseCorrectSuffixHighlighting(id, suffix));
            }

            // special cases
            // dictionary
            AnalyzeWrongClassName(element, consumer, "Dictionary", "System.Collections.IDictionary", (id, suffix) => new UseCorrectSuffixHighlighting(id, suffix));
            AnalyzeWrongClassName(element, consumer, "Dictionary", "System.Collections.Generic.IDictionary", (id, suffix) => new UseCorrectSuffixHighlighting(id, suffix));

            // collections
            AnalyzeWrongClassName(element, consumer, "Collection", "System.Collections.ICollection", (id, suffix) => new UseCorrectSuffixHighlighting(id, suffix));
            AnalyzeWrongClassName(element, consumer, "Collection", "System.Collections.IEnumerable", (id, suffix) => new UseCorrectSuffixHighlighting(id, suffix));
            AnalyzeWrongClassName(element, consumer, "Collection", "System.Collections.Generic.ICollection", (id, suffix) => new UseCorrectSuffixHighlighting(id, suffix));
            AnalyzeWrongClassName(element, consumer, "Collection", "System.Collections.Generic.IEnumerable", (id, suffix) => new UseCorrectSuffixHighlighting(id, suffix));
        }

        private void Analyze(IClassDeclaration element, IHighlightingConsumer consumer, string suffix, string baseClass,
            Func<ICSharpIdentifier, string, IHighlighting> highlighting)
        {
            AnalyzeWrongClassName(element, consumer, suffix, baseClass, highlighting);
            AnalyzeNotImplement(element, consumer, suffix, baseClass, highlighting);
        }

        private void AnalyzeWrongClassName(IClassDeclaration element, IHighlightingConsumer consumer, string suffix, string baseClass, Func<ICSharpIdentifier, string, IHighlighting> highlighting)
        {
            string[] suffixes = suffix.Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries);
            if (suffixes.Length == 0)
                throw new ArgumentException("Rule doesn't have suffix.", suffix);

            // TODO refactor it later
            string errorMsg;
            if (suffix.Length == 1)
                errorMsg = string.Format("Naming: Class doesn't have {0} suffix.", suffix);
            else
            {
                // TODO move it to helper class
                var sb = new StringBuilder();
                for (int i = 0; i < suffixes.Length; i++)
                {
                    sb.Append(suffixes[i]);
                    if (i != suffixes.Length - 1)
                        sb.Append(" or ");
                }

                errorMsg = string.Format("Naming: Class doesn't have {0} suffix.", sb);
            }

            // 1. get exntend list
            IExtendsList extends = element.ExtendsList;
            if (extends == null)
                return;
            
            // 2. check if class extends Attribute class, it has to have Attbite suffix
            if (!AnalyzerHelper.IsImplement(extends, baseClass))
                return;

            // 3. get class name
            string name = element.NameIdentifier.Name;
//            if (!string.IsNullOrEmpty(name) && !name.EndsWith(suffix))
            if (!string.IsNullOrEmpty(name) && !suffixes.Any(name.EndsWith))
            {
                ICSharpIdentifier identifier = element.NameIdentifier;
                consumer.AddHighlighting(highlighting(identifier, errorMsg), identifier.GetDocumentRange(), identifier.GetContainingFile());
            }
        }

        private void AnalyzeNotImplement(IClassDeclaration element, IHighlightingConsumer consumer, string suffix, string baseClass,
            Func<ICSharpIdentifier, string, IHighlighting> highlighting)
        {
            string errorMsg = string.Format("Naming: Has {0} suffix but doesn't extend {1} class", suffix, baseClass); 

            // 1. get class name
            if (element.NameIdentifier == null)
                return;

            string name = element.NameIdentifier.Name;

            if (!string.IsNullOrEmpty(name) && !name.EndsWith(suffix))
                return;

            // 2. get exntend list
            IExtendsList extends = element.ExtendsList;
            if (extends == null)
            {
                ICSharpIdentifier nameIdentifier = element.NameIdentifier;
                consumer.AddHighlighting(highlighting(nameIdentifier, errorMsg), nameIdentifier.GetDocumentRange(), nameIdentifier.GetContainingFile());
                return;
            }

            // 3. check if class has Attribute suffix but doesn't extend Attribute class
            if (!AnalyzerHelper.IsImplement(extends, baseClass))
            {
                ICSharpIdentifier nameIdentifier = element.NameIdentifier;
                consumer.AddHighlighting(highlighting(nameIdentifier, errorMsg), nameIdentifier.GetDocumentRange(), nameIdentifier.GetContainingFile());
            }
        }

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<NamingRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.UseCorrectSuffixEnabled;
        }
    }
}