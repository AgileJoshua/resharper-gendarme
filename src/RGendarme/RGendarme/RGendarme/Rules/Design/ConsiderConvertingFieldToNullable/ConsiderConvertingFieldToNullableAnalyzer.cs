using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Design.ConsiderConvertingFieldToNullable
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(ConsiderConvertingFieldToNullableHighlighting) })]
    public class ConsiderConvertingFieldToNullableAnalyzer : ElementProblemAnalyzer<IClassDeclaration>
    {
        private readonly ISettingsStore _settings;

        public ConsiderConvertingFieldToNullableAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (element.Body == null)
                return;

            const string pattern = @"^(is|has)";

            var boolFieldses = new List<string>();
            var possibleFields = new List<IFieldDeclaration>();
            foreach (IFieldDeclaration field in element.FieldDeclarationsEnumerable)
            {
                if (field.Type == null || field.NameIdentifier == null || string.IsNullOrEmpty(field.NameIdentifier.Name))
                    continue;

                string type = field.Type.ToString();
                string name = field.NameIdentifier.Name;

                bool starts = Regex.IsMatch(name, pattern, RegexOptions.IgnoreCase);

                if (type.Equals("System.Boolean") && starts)
                {
                    string shortName = Regex.Replace(name, pattern, string.Empty, RegexOptions.IgnoreCase);
                    boolFieldses.Add(shortName);
                    continue;
                }

                if (!field.Type.IsNullable())
                {
                    possibleFields.Add(field);
                }
            }

            foreach (string boolField in boolFieldses)
            {
                IFieldDeclaration replaced = possibleFields.FirstOrDefault(
                    f => string.Compare(f.NameIdentifier.Name, boolField, StringComparison.OrdinalIgnoreCase) == 0);
                if (replaced != null)
                {
                    consumer.AddHighlighting(new ConsiderConvertingFieldToNullableHighlighting(element, string.Format("Merge {0} and {1} variables.", boolField, replaced.NameIdentifier.Name)), replaced.GetDocumentRange(), element.GetContainingFile());
                }
            }

            //throw new System.NotImplementedException();
        }
    }

    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class ConsiderConvertingFieldToNullableHighlighting : IHighlighting
    {
        public IClassDeclaration Declaration { get; private set; }
        private readonly string _warningMessage;

        public ConsiderConvertingFieldToNullableHighlighting(IClassDeclaration declaration, string warningMessage)
        {
            Declaration = declaration;
            _warningMessage = warningMessage;
        }

        public bool IsValid()
        {
            return Declaration != null && Declaration.IsValid();
        }

        public string ToolTip { get { return string.Format("Design: consider converting field to nullable. {0}", _warningMessage); } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch { get { return 0; } }
    }
}