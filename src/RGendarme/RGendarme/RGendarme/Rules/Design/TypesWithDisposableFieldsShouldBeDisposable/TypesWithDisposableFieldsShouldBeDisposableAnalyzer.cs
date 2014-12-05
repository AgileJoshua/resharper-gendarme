using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using RGendarme.Lib;
using RGendarme.Lib.Extenstions;

namespace RGendarme.Rules.Design.TypesWithDisposableFieldsShouldBeDisposable
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(TypesWithDisposableFieldsShouldBeDisposableHighlighting) })]
    public class TypesWithDisposableFieldsShouldBeDisposableAnalyzer : ElementProblemAnalyzer<IClassDeclaration>
    {
        private readonly ISettingsStore _settings;
        public TypesWithDisposableFieldsShouldBeDisposableAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (element.NameIdentifier == null || element.FieldDeclarations.IsEmpty)
                return;

            // 1. check for disposed fields
            IDeclaredType disposedType = TypeFactory.CreateTypeByCLRName(typeof(System.IDisposable).FullName, element.GetPsiModule(), element.GetResolveContext());

            bool hasDisposedFields = false;
            foreach (IFieldDeclaration field in element.FieldDeclarationsEnumerable)
            {
                if (field.DeclaredElement == null || !field.DeclaredElement.IsValid())
                    continue;

                if (field.DeclaredElement.Type.IsImplicitlyConvertibleTo(disposedType,
                    ClrPredefinedTypeConversionRule.INSTANCE))
                {
                    hasDisposedFields = true;
                    break;
                }
            }

            if (!hasDisposedFields)
                return;

            // 2. has dispose method
            bool hasDisposeMethod = false;
            foreach (IMethodDeclaration method in element.MethodDeclarationsEnumerable)
            {
                if (!string.IsNullOrEmpty(method.DeclaredName)
                    && method.DeclaredName.Equals("Dispose")
                    && method.ParameterDeclarations.IsEmpty
                    && method.IsPublic()
                    && !method.IsAbstract())
                {
                    hasDisposeMethod = true;
                    break;
                }
            }

            bool implemenetDisposeInterface = AnalyzerHelper.IsImplement(element, typeof (System.IDisposable));

            if (!implemenetDisposeInterface || !hasDisposeMethod)
            {
                consumer.AddHighlighting(new TypesWithDisposableFieldsShouldBeDisposableHighlighting(element),
                    element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }
        }
    }
}