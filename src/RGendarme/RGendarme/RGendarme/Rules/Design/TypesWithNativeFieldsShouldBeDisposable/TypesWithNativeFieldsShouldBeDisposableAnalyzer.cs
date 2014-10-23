using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib;

namespace RGendarme.Rules.Design.TypesWithNativeFieldsShouldBeDisposable
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(TypesWithNativeFieldsShouldBeDisposableHighlighting) })]
    public class TypesWithNativeFieldsShouldBeDisposableAnalyzer : ElementProblemAnalyzer<IClassDeclaration>
    {
        private readonly ISettingsStore _settings;
        public TypesWithNativeFieldsShouldBeDisposableAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (element.FieldDeclarations.IsEmpty)
                return;

            // 1. if class has  IntPtr, UIntPtr, or HandleRef fields - throw warning
            var unsafeTypes = new[] {"System.IntPtr", "System.UIntPtr", "System.Runtime.InteropServices.HandleRef"};
            bool hasUnsafeFields = false; //= element.FieldDeclarationsEnumerable.Any(field => field.IsUnsafe);
            foreach (IFieldDeclaration field in element.FieldDeclarationsEnumerable)
            {
                string clrName = field.DeclaredElement.Type.ToString();
                if (unsafeTypes.Any(clrName.Equals))
                {
                    hasUnsafeFields = true;
                    break;
                }
            }

            if (!hasUnsafeFields)
                return;

            // 2. if class implements IDisposable and Dispose exists and is not abstract - return
            bool hasDispose = false; // if true - return
            if (element.ExtendsList != null && AnalyzerHelper.IsImplement(element.ExtendsList, "System.IDisposable"))
            {
                // TODO refactor this - move it to helper class
                foreach (IMethodDeclaration m in element.MethodDeclarationsEnumerable)
                {
                    if (m.NameIdentifier != null && !string.IsNullOrEmpty(m.NameIdentifier.Name) &&
                        m.NameIdentifier.Name.Equals("Dispose") &&
                        m.ModifiersList != null &&
                        !m.ModifiersList.HasModifier(CSharpTokenType.ABSTRACT_KEYWORD))
                    {
                        hasDispose = true;
                        break;
                    }
                }
            }

            if (!hasDispose)
            {
                consumer.AddHighlighting(new TypesWithNativeFieldsShouldBeDisposableHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }
        }
    }
}