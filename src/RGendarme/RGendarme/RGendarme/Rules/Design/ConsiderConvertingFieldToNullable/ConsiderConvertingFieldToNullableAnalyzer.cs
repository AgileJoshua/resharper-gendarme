using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
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

            // 1. Get all bool fields and all not nullable fields
            var boolFieldses = new List<IFieldDeclaration>();
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
                    boolFieldses.Add(field);
                    continue;
                }

                if (!field.Type.IsNullable())
                {
                    possibleFields.Add(field);
                }
            }

            // 2. check if bool and not bool are similar
            foreach (IFieldDeclaration boolField in boolFieldses)
            {
                string shortName = Regex.Replace(boolField.NameIdentifier.Name, pattern, string.Empty, RegexOptions.IgnoreCase);

                IFieldDeclaration replaced = possibleFields.FirstOrDefault(
                    f => string.Compare(f.NameIdentifier.Name, shortName, StringComparison.OrdinalIgnoreCase) == 0);

                if (replaced != null)
                {
                    string warning = string.Format("Merge {0} and {1} fields.", boolField.NameIdentifier.Name, replaced.NameIdentifier.Name);

                    ThrowWarning(replaced, consumer, warning);
                    ThrowWarning(boolField, consumer, warning);
                }
            }
        }

        private void ThrowWarning(IFieldDeclaration field, IHighlightingConsumer consumer, string warning)
        {
            consumer.AddHighlighting(new ConsiderConvertingFieldToNullableHighlighting(field, warning), field.GetDocumentRange(), field.GetContainingFile());
        }
    }
}