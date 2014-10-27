using JetBrains.Metadata.Reader.API;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Modules;

namespace RGendarme.Lib
{
    internal static class TypeHelper
    {
        public static bool TryGetTypeElement(string typeName, IPsiModule psiModule, IModuleReferenceResolveContext moduleReferenceResolveContext, out ITypeElement typeElement)
        {
            typeElement = TypeFactory.CreateTypeByCLRName(typeName, psiModule, moduleReferenceResolveContext).GetTypeElement();
            return typeElement != null;
        }

//        public static bool TryGetType(string typeName, IPsiModule psiModule, IModuleReferenceResolveContext moduleReferenceResolveContext, out IDeclaredType typeElement)
//        {
//            typeElement = TypeFactory.CreateTypeByCLRName(typeName, psiModule, moduleReferenceResolveContext);
//            return typeElement != null;
//        }
    }
}