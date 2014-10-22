using System;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;

namespace RGendarme.Lib
{
    public static class AnalyzerHelper
    {
        /// <summary>
        /// Check if extend list contains specific type.
        /// </summary>
        /// <param name="extends">Extend list</param>
        /// <param name="baseClass">Full CLR type name.</param>
        /// <returns></returns>
        public static bool IsImplement(IExtendsList extends, string baseClass)
        {
            bool result = false;
            foreach (IDeclaredTypeUsage type in extends.ExtendedInterfaces)
            {
                var declaredType = type as IUserDeclaredTypeUsage;
                if (declaredType != null && declaredType.TypeName != null)
                {
                    ResolveResultWithInfo resolveResult = declaredType.TypeName.Reference.CurrentResolveResult;
                    if (resolveResult != null)
                    {
                        IDeclaredElement element = resolveResult.Result.DeclaredElement;
                        if (element != null)
                        {
                            var cls = element as IClass;
                            if (cls != null)
                            {
                                if (cls.GetClrName().FullName.Equals(baseClass))
                                {
                                    result = true;
                                    break;
                                }
                            }

                            var inter = element as IInterface;
                            if (inter != null)
                            {
                                string fullName = inter.GetClrName().FullName;
                                if (string.IsNullOrWhiteSpace(fullName)) continue;

                                // When interface is generic - remove '`2' at the end.
                                int pos = fullName.LastIndexOf("`", StringComparison.OrdinalIgnoreCase);
                                if (pos != -1)
                                {
                                    fullName = fullName.Substring(0, pos);
                                }

                                if (fullName.Equals(baseClass))
                                {
                                    result = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        public static bool HasAttribute(ICSharpTypeDeclaration declaration, string baseClass)
        {
            var attributes = declaration.Attributes;
            if (attributes.Count == 0)
                return false;

            bool hasFlagsAttribute = false;
            foreach (IAttribute attr in attributes)
            {
                IReferenceName name = attr.Name;
                if (name == null) continue;

                var result = name.Reference.CurrentResolveResult;
                if (result == null) continue;

                var cls = result.DeclaredElement as IClass;
                if (cls == null) continue;

                var clrName = cls.GetClrName().FullName;
                if (clrName.Equals(baseClass))
                {
                    hasFlagsAttribute = true;
                    break;
                }
            }

            return hasFlagsAttribute;
        }
    }
}