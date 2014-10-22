using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace RGendarme.Lib.Extenstions
{
    public static class ParameterDeclarationExtensions
    {
        /// <summary>
        /// Check if param has 'ref' modifier
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool IsRef(this ICSharpParameterDeclaration param)
        {
            if (param == null)
                return false;

            var regularParam = param as IRegularParameterDeclaration;
            if (regularParam == null)
                return false;

            return regularParam.Mode != null && regularParam.Mode.NodeType == CSharpTokenType.REF_KEYWORD;
        }

        /// <summary>
        /// Check if param has 'out' modifier
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool IsOut(this ICSharpParameterDeclaration param)
        {
            if (param == null)
                return false;

            var regularParam = param as IRegularParameterDeclaration;
            if (regularParam == null)
                return false;

            return regularParam.Mode != null && regularParam.Mode.NodeType == CSharpTokenType.OUT_KEYWORD;
        }
    }
}