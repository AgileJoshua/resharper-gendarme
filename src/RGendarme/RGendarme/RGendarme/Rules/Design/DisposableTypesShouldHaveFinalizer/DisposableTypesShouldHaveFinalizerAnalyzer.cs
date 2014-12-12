using System;
using System.Linq;
using JetBrains.Application.DataContext;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib;
using RGendarme.Settings.Design;

namespace RGendarme.Rules.Design.DisposableTypesShouldHaveFinalizer
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(DisposableTypesShouldHaveFinalizerHighlighting) })]
    public class DisposableTypesShouldHaveFinalizerAnalyzer : ElementProblemAnalyzer<IClassDeclaration>, IRGendarmeRule
    {
        private readonly ISettingsStore _settings;

        public DisposableTypesShouldHaveFinalizerAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!IsEnabled(element.ToDataContext()))
                return;

            if (!AnalyzerHelper.IsImplement(element, "System.IDisposable") || element.FieldDeclarations.IsEmpty)
                return;

#warning do not use magic strings
            var unsafeTypes = new[] { "System.IntPtr", "System.UIntPtr", "System.Runtime.InteropServices.HandleRef" };
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


            if (element.DestructorDeclarations.IsEmpty)
            {
                consumer.AddHighlighting(new DisposableTypesShouldHaveFinalizerHighlighting(element), element.NameIdentifier.GetDocumentRange(), element.GetContainingFile());
            }
        }

        public bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx)
        {
            var boundSettings = _settings.BindToContextTransient(ContextRange.Smart(ctx));
            var setting = boundSettings.GetKey<DesignRulesSettings>(SettingsOptimization.OptimizeDefault);

            return setting.DisposableTypesShouldHaveFinalizerEnabled;
        }
    }
}