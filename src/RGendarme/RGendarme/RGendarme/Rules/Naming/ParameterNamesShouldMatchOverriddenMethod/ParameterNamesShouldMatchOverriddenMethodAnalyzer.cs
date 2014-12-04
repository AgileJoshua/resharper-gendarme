using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using RGendarme.Lib.Extenstions;

namespace RGendarme.Rules.Naming.ParameterNamesShouldMatchOverriddenMethod
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(ParameterNamesShouldMatchOverriddenMethodHighlighting) })]
    public class ParameterNamesShouldMatchOverriddenMethodAnalyzer : ElementProblemAnalyzer<IClassDeclaration>
    {
        private readonly ISettingsStore _settings;
        public ParameterNamesShouldMatchOverriddenMethodAnalyzer(ISettingsStore settings)
        {
            _settings = settings;
        }

        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (element.ExtendsList == null || element.ExtendsList.IsEmpty() || element.MethodDeclarations.IsEmpty)
                return;

            foreach (IDeclaredTypeUsage type in element.ExtendsList.ExtendedInterfacesEnumerable)
            {
                var userType = type as IUserDeclaredTypeUsage;
                if (userType == null)
                    continue;

                IReferenceName typeName = userType.TypeName;
                if (typeName == null) 
                    continue;

                if (typeName.Reference.CurrentResolveResult == null)
                    continue;

                var  inter = typeName.Reference.CurrentResolveResult.Result.DeclaredElement as IInterface;
                if (inter == null)
                    continue;

                var declarations = inter.GetDeclarations();
                foreach (IDeclaration decl in declarations)
                {
                    var interfaceDeclaration = decl as IInterfaceDeclaration;
                    if (interfaceDeclaration == null)
                        continue;

                    AnalyzeImplementedMethods(element, interfaceDeclaration, consumer);
                }
            }
        }

        private void AnalyzeImplementedMethods(IClassDeclaration classDeclaration, IInterfaceDeclaration interfaceDeclaration, IHighlightingConsumer consumer)
        {
            if (classDeclaration.MethodDeclarations.IsEmpty || interfaceDeclaration.MethodDeclarations.IsEmpty)
                return;

            foreach (IMethodDeclaration interfaceMethod in interfaceDeclaration.MethodDeclarationsEnumerable)
            {
                IMethodDeclaration method = interfaceMethod;
                if (string.IsNullOrEmpty(method.DeclaredName) || method.ParameterDeclarations.IsEmpty)
                    continue;

                IList<IMethodDeclaration> classMethods =
                    classDeclaration.MethodDeclarations.Where(
                        m =>
                            !string.IsNullOrEmpty(m.DeclaredName) &&
                            m.DeclaredName.Equals(method.DeclaredName)).ToList();

                foreach (IMethodDeclaration findedMethod in classMethods)
                {
                    AnalyzeInputParams(method, findedMethod, consumer);
                }
            }
        }

        private void AnalyzeInputParams(IMethodDeclaration interfaceMethod, IMethodDeclaration classMethod,
            IHighlightingConsumer consumer)
        {
            if (!interfaceMethod.HasSameSignatureAs(classMethod) 
                || interfaceMethod.ParameterDeclarations.Count != classMethod.ParameterDeclarations.Count 
                || interfaceMethod.ParameterDeclarations.IsEmpty)
                return;

            for (int i = 0; i < interfaceMethod.ParameterDeclarations.Count; i++)
            {
                ICSharpParameterDeclaration interfaceParam = interfaceMethod.ParameterDeclarations[i];
                ICSharpParameterDeclaration classParam = classMethod.ParameterDeclarations[i];

                if (!interfaceParam.DeclaredName.Equals(classParam.DeclaredName) && classParam.NameIdentifier != null)
                {
                    consumer.AddHighlighting(new ParameterNamesShouldMatchOverriddenMethodHighlighting(classParam, interfaceParam.DeclaredName), classParam.NameIdentifier.GetDocumentRange(), classParam.GetContainingFile());
                }
            }
        }
    }
}