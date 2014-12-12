using System;
using System.Linq;
using JetBrains.Application.DataContext;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib;
using RGendarme.Settings.Design;

namespace RGendarme.Rules.Design.EnumsShouldUseInt32
{
    [ElementProblemAnalyzer(new[] { typeof(IEnumDeclaration) }, HighlightingTypes = new[] { typeof(EnumsShouldUseInt32Highlighting) })]
    public class EnumsShouldUseInt32Analyzer : ElementProblemAnalyzer<IEnumDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        public EnumsShouldUseInt32Analyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IEnumDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            var underlinedType = element.UnderlyingTypeUsage as IPredefinedDeclaredTypeUsage;
            if (underlinedType == null)
                return;

            IPredefinedTypeReference typeName = underlinedType.PredefinedTypeName;
            if (typeName == null || typeName.Reference == null)
                return;

            var result = typeName.Reference.Resolve();
            IDeclaredElement declaredElement = result.DeclaredElement;
            if (declaredElement == null)
                return;

#if false // to find solution without magic strings
//            IType baseType = declaredElement.Type();
//            if (baseType == null)
//                return;
//
//            IDeclaredType[] availableTypes =
//            {
//                TypeFactory.CreateTypeByCLRName(typeof(System.Byte).FullName, element.GetPsiModule(), element.GetResolveContext()),
//                TypeFactory.CreateTypeByCLRName(typeof(System.Int16).FullName, element.GetPsiModule(), element.GetResolveContext()),
//                TypeFactory.CreateTypeByCLRName(typeof(System.Int32).FullName, element.GetPsiModule(), element.GetResolveContext()),
//                TypeFactory.CreateTypeByCLRName(typeof(System.Int64).FullName, element.GetPsiModule(), element.GetResolveContext())
//            };
//
//            if (!availableTypes.Any(
//                    type => baseType.IsImplicitlyConvertibleTo(type, ClrPredefinedTypeConversionRule.INSTANCE)))
//            {
//                consumer.AddHighlighting(new EnumsShouldUseInt32Highlighting(element), element.UnderlyingTypeUsage.GetDocumentRange(), element.GetContainingFile());
//            }
#endif

            string[] availablyTypeNames =
            {
                typeof(System.Byte).FullName,
                typeof(System.Int16).FullName,
                typeof(System.Int32).FullName,
                typeof(System.Int64).FullName
            };

            // TODO: do not compare types by string - it's error prone
            string baseTypeName = declaredElement.ToString();
            if (!availablyTypeNames.Any(baseTypeName.EndsWith))
            {
                consumer.AddHighlighting(new EnumsShouldUseInt32Highlighting(element), element.UnderlyingTypeUsage.GetDocumentRange(), element.GetContainingFile());
            }
        }

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<DesignRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.EnumsShouldUseInt32Enabled;
        }
    }
}