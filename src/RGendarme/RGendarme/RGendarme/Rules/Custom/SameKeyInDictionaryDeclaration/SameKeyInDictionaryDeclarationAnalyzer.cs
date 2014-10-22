using System.Collections.Generic;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Custom.SameKeyInDictionaryDeclaration
{
    [ElementProblemAnalyzer(new[] { typeof(ILocalVariableDeclaration) }, HighlightingTypes = new[] { typeof(SameKeyInDictionaryDeclarationHighlight) })]
    public class SameKeyInDictionaryDeclarationAnalyzer : ElementProblemAnalyzer<ILocalVariableDeclaration>
    {
        private readonly ISettingsStore _settings;
        public SameKeyInDictionaryDeclarationAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(ILocalVariableDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (element.Initial == null)
                return;

            var initializer = element.Initial as IExpressionInitializer;
            if (initializer == null)
                return;

            var creation = initializer.Value as IObjectCreationExpression;
            if (creation == null)
                return;

            // 1. Is it generic dictionary?
            var dictType = creation.CreatedTypeUsage as IUserDeclaredTypeUsage;
            if (dictType == null || dictType.TypeName == null || dictType.TypeName.Reference.CurrentResolveResult == null)
                return;

            var cls = dictType.TypeName.Reference.CurrentResolveResult.Result.DeclaredElement as IClass;
            if (cls == null || !cls.GetClrName().FullName.Equals("System.Collections.Generic.Dictionary`2"))
                return;

            // 2. Is it dictionary initializer?
            var dictInitializer = creation.Initializer as ICollectionInitializer;
            if (dictInitializer == null || dictInitializer.ElementInitializers.Count == 0)
                return;

            var dict = new Dictionary<object, ICollectionElementInitializer>();
            foreach (ICollectionElementInitializer item in dictInitializer.ElementInitializersEnumerable)
            {
                if (item.Arguments.Count == 0) 
                    continue;

                // 3. get key value
                ICSharpArgument first = item.Arguments[0]; 
                if (first.Value == null) 
                    continue;

                var value = first.Value.ConstantValue.Value;
                if (value == null)
                    continue;

                if (!dict.ContainsKey(value))
                {
                    dict.Add(value, item);
                }
                else
                {
                    // if the same key already defined
                    consumer.AddHighlighting(new SameKeyInDictionaryDeclarationHighlight(element), item.GetDocumentRange(), element.GetContainingFile());
                }
            }
        }
    }
}