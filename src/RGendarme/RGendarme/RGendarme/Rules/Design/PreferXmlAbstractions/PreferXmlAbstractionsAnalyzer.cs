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
using JetBrains.ReSharper.Psi.Util;
using RGendarme.Lib;
using RGendarme.Lib.Extenstions;
using RGendarme.Settings.Design;

namespace RGendarme.Rules.Design.PreferXmlAbstractions
{
    [ElementProblemAnalyzer(new []{typeof(IClassDeclaration)}, HighlightingTypes = new []{typeof(PreferXmlAbstractionsHighlighting)})]
    public class PreferXmlAbstractionsAnalyzer : ElementProblemAnalyzer<IClassDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        public PreferXmlAbstractionsAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            if (element.NameIdentifier == null || (element.PropertyDeclarations.IsEmpty && element.MemberDeclarations.IsEmpty))
                return;

            IDeclaredType[] deniedTypes = 
            {
                TypeFactory.CreateTypeByCLRName(typeof(System.Xml.XmlDocument).FullName, element.GetPsiModule(), element.GetResolveContext()),

                TypeFactory.CreateTypeByCLRName(typeof(System.Xml.XPath.XPathDocument).FullName, element.GetPsiModule(), element.GetResolveContext()),

                TypeFactory.CreateTypeByCLRName(typeof(System.Xml.XmlNode).FullName, element.GetPsiModule(), element.GetResolveContext())
            };
            
            foreach (IPropertyDeclaration property in element.PropertyDeclarationsEnumerable)
            {
                AnalyzeProperty(property, consumer, deniedTypes);
            }

            foreach (IMethodDeclaration method in element.MethodDeclarationsEnumerable)
            {
                AnalyzeMethod(method, consumer, deniedTypes);
            }
        }

        private void AnalyzeProperty([NotNull]IPropertyDeclaration property, IHighlightingConsumer consumer, IEnumerable<IDeclaredType> deniedTypes)
        {
            if (!property.IsPublic() || property.NameIdentifier == null || property.DeclaredElement == null)
                return;

            if (deniedTypes.Any(
                    t =>
                        property.DeclaredElement.ReturnType.IsImplicitlyConvertibleTo(t,
                            ClrPredefinedTypeConversionRule.INSTANCE)))
            {
                // todo highlight return type
                consumer.AddHighlighting(new PreferXmlAbstractionsHighlighting(property), property.NameIdentifier.GetDocumentRange(), property.GetContainingFile());
            }
        }

        private void AnalyzeMethod([NotNull]IMethodDeclaration method, IHighlightingConsumer consumer, IDeclaredType[] deniedTypes)
        {
            if (!method.IsPublic() || method.ParameterDeclarations.IsEmpty)
                return;

            foreach (ICSharpParameterDeclaration param in method.ParameterDeclarations)
            {
                if (param.DeclaredElement == null)
                    continue;

                if (deniedTypes.Any(
                        t =>
                            param.DeclaredElement.Type.IsImplicitlyConvertibleTo(t,
                                ClrPredefinedTypeConversionRule.INSTANCE)))
                {
                    // todo highlight return type
                    consumer.AddHighlighting(new PreferXmlAbstractionsHighlighting(param), param.NameIdentifier.GetDocumentRange(), param.GetContainingFile());
                }
            }
        }

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<DesignRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.PreferXmlAbstractionsEnabled;
        }
    }
}