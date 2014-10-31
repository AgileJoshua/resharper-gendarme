using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using RGendarme.Lib;

namespace RGendarme.Rules.Design.AttributeArgumentsShouldHaveAccessors
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(AttributeArgumentsShouldHaveAccessorsighlighting) })]
    public class AttributeArgumentsShouldHaveAccessorsAnalyzer : ElementProblemAnalyzer<IClassDeclaration>
    {
        private readonly ISettingsStore _settings;
        public AttributeArgumentsShouldHaveAccessorsAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!AnalyzerHelper.IsImplement(element, "System.Attribute") || element.FieldDeclarations.IsEmpty)
                return;

            // todo get fields only that assinged in constructor

            foreach (IFieldDeclaration field in element.FieldDeclarationsEnumerable)
            {
                bool isReturnedFromProperty = false;
                foreach (IPropertyDeclaration property in element.PropertyDeclarationsEnumerable)
                {
                    if (IsReturnField(property, field))
                    {
                        isReturnedFromProperty = true;
                        break;
                    }
                }

                if (!isReturnedFromProperty)
                {
                    consumer.AddHighlighting(new AttributeArgumentsShouldHaveAccessorsighlighting(element), field.GetDocumentRange(), field.GetContainingFile());
                }
            }
        }

        private bool IsReturnField(IPropertyDeclaration property, IFieldDeclaration field)
        {
            Assertion.Assert(property != null, "property != null");
            Assertion.Assert(field != null, "field != null");

            if (property.AccessorDeclarations.IsEmpty)
                return false;

            IAccessorDeclaration get = property.AccessorDeclarationsEnumerable.FirstOrDefault(accessor => accessor.Kind == AccessorKind.GETTER);

            if (get == null || get.Body == null || get.Body.Statements.IsEmpty)
                return false;

            bool isReturnField = false;
            foreach (ICSharpStatement statement in get.Body.StatementsEnumerable)
            {
                var ret = statement as IReturnStatement;
                if (ret == null || ret.Value == null)
                    continue;

                var expr = ret.Value as IReferenceExpression;
                if (expr == null)
                    continue;

                var f = expr.Reference.Resolve().Result.DeclaredElement as IField;
                if (f != null && f.ShortName.Equals(field.NameIdentifier.Name))
                {
                    isReturnField = true;
                    break;
                }

            }

            return isReturnField;
        }
    }
}