using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace RGendarme.Lib.Extenstions
{
    public static class MethodDeclarationExtensions
    {
        public static bool HasSameSignatureAs(this IMethodDeclaration first, IMethodDeclaration second)
        {
            Assertion.Assert(first != null, "first != null");
            Assertion.Assert(second != null, "second != null");

            if (first.DeclaredElement == null || second.DeclaredElement == null)
                return false;

            // 1. check name (casesensetive)
            if (first.NameIdentifier != null && second.NameIdentifier != null &&
                !first.NameIdentifier.Name.Equals(second.NameIdentifier.Name))
                return false;

            // 2. check inputparams count
            if (first.ParameterDeclarations.Count != second.ParameterDeclarations.Count)
                return false;

            // 3. check input params type
            for (int i = 0; i < first.ParameterDeclarations.Count; i++)
            {
                IParameterDeclaration firstParam = first.ParameterDeclarations[i];
                IParameterDeclaration secondParam = second.ParameterDeclarations[i];

                if (firstParam.DeclaredElement == null || secondParam.DeclaredElement == null)
                    return false;

                string firstParamReturnType = firstParam.DeclaredElement.Type.ToString();
                string secondParamReturnType = secondParam.DeclaredElement.Type.ToString();

                if (!firstParamReturnType.Equals(secondParamReturnType))
                    return false;
            }

            // 4. check return types
            string firstTypeName = first.Type.ToString();
            string secondTypeName = second.Type.ToString();
            if (!firstTypeName.Equals(secondTypeName))
                return false;

            return true;
        }
    }
}