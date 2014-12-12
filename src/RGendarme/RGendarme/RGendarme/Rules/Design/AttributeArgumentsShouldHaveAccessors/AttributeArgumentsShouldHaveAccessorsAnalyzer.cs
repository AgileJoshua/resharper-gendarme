using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application.DataContext;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using RGendarme.Lib;
using RGendarme.Settings.Design;

namespace RGendarme.Rules.Design.AttributeArgumentsShouldHaveAccessors
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(AttributeArgumentsShouldHaveAccessorsighlighting) })]
    public class AttributeArgumentsShouldHaveAccessorsAnalyzer : ElementProblemAnalyzer<IClassDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;
        public AttributeArgumentsShouldHaveAccessorsAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            if (!AnalyzerHelper.IsImplement(element, "System.Attribute") || element.FieldDeclarations.IsEmpty || element.ConstructorDeclarations.IsEmpty)
                return;

            // 1. Get all fields that do not bind to properties
            var fieldsWithoutProps = new List<IFieldDeclaration>();
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
                    fieldsWithoutProps.Add(field);
                }
            }

            foreach (IConstructorDeclaration constructor in element.ConstructorDeclarationsEnumerable)
            {
                if (constructor.ParameterDeclarations.IsEmpty)
                    continue;

                foreach (ICSharpParameterDeclaration p in constructor.ParameterDeclarationsEnumerable)
                {
                    if (p.DeclaredElement == null || string.IsNullOrEmpty(p.DeclaredElement.ShortName))
                        continue;

                    if (IsAssignedToField(constructor, p.DeclaredElement.ShortName, fieldsWithoutProps))
                    {
                        // todo add consumer
                        consumer.AddHighlighting(new AttributeArgumentsShouldHaveAccessorsighlighting(element), p.GetDocumentRange(), element.GetContainingFile());
                    }
                }
            }
        }

        private bool IsAssignedToField([NotNull]IConstructorDeclaration constructor, string paramName, [NotNull]IList<IFieldDeclaration> fields)
        {
            Assertion.Assert(constructor != null, "constructor != null");
            Assertion.Assert(fields != null, "field != null");

            if (constructor.Body == null || constructor.Body.Statements.IsEmpty)
                return false;

            bool isAssignedToField = false;
            foreach (ICSharpStatement statement in constructor.Body.StatementsEnumerable)
            {
                var expr = statement as IExpressionStatement;
                if (expr == null || expr.Expression == null)
                    continue;

                var assignment = expr.Expression as IAssignmentExpression;
                if (assignment == null || assignment.AssignmentType != AssignmentType.EQ)
                    continue;

                // ----- [destination must be field] -----
                var destRef = assignment.Dest as IReferenceExpression;
                if (destRef == null)
                    continue;

                var destField = destRef.Reference.Resolve().Result.DeclaredElement as IField;
                if (destField == null)
                    continue;

                if (!fields.Any(x => x.DeclaredElement.ShortName.Equals(destField.ShortName)))
                    continue;
                // ----- [/destination must be field] -----

                // ----- [source must construct input parameter] -----
                var sourceRef = assignment.Source as IReferenceExpression;
                if (sourceRef == null)
                    continue;

                var sourceInputParam = sourceRef.Reference.Resolve().Result.DeclaredElement as IParameter;
                if (sourceInputParam == null)
                    continue;

                if (sourceInputParam.ShortName.Equals(paramName))
                {
                    isAssignedToField = true;
                    break;
                }
                // ----- [source must construct input parameter] -----
            }

            return isAssignedToField;
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

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<DesignRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.AttributeArgumentsShouldHaveAccessorsEnabled;
        }
    }
}