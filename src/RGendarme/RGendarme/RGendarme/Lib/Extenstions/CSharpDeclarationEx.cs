using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Lib.Extenstions
{
    public static class CSharpDeclarationEx
    {
        public static bool IsPublic(this ICSharpModifiersOwnerDeclaration declaration)
        {
            if (declaration.ModifiersList == null || declaration.ModifiersList.IsEmpty())
                return false;

            return declaration.ModifiersList.HasModifier(CSharpTokenType.PUBLIC_KEYWORD);
        }
    }
}