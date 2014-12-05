using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.Util;

namespace RGendarme.Lib
{
    public static class AnalyzerHelper
    {
        public static bool IsImplement(IClassDeclaration declaration, Type baseClass)
        {
            Assertion.Assert(baseClass != null, "baseClass != null");
            return IsImplement(declaration, baseClass.FullName);

        }

        [Obsolete("Do not use method with magic strings. Use with Type baseClass instead.")]
        public static bool IsImplement(IClassDeclaration declaration, string baseClass)
        {
            Assertion.Assert(declaration != null, "declaration != null");

            if (declaration == null)
                throw new ArgumentNullException("declaration");

            if (declaration.ExtendsList == null)
                return false;

            return IsImplement(declaration.ExtendsList, baseClass);
        }

        public static bool IsImplement([NotNull]IClassDeclaration declaration, [NotNull]IInterfaceDeclaration inter)
        {
            Assertion.Assert(declaration != null, "declaration != null");
            Assertion.Assert(inter != null, "inter != null");

            if (declaration.ExtendsList == null || inter.NameIdentifier == null || string.IsNullOrEmpty(inter.NameIdentifier.Name))
                return false;

            bool isImplemented = false;
            foreach (IDeclaredTypeUsage type in declaration.ExtendsList.ExtendedInterfacesEnumerable)
            {
                var declaredType = type as IUserDeclaredTypeUsage;
                if (declaredType == null || declaredType.TypeName == null)
                    continue;

                ResolveResultWithInfo resolveResult = declaredType.TypeName.Reference.CurrentResolveResult;
                if (resolveResult == null)
                    continue;

                var element = resolveResult.Result.DeclaredElement as IInterface;
                if (element != null)
                {
                    string fullClrName = element.GetClrName().FullName;
                    if (fullClrName.Equals(inter.CLRName))
                    {
                        isImplemented = true;
                        break;
                    }
                }
            }
            
            return isImplemented;
        }

        /// <summary>
        /// Check if extend list contains specific type.
        /// </summary>
        /// <param name="extends">Extend list</param>
        /// <param name="baseClass">Full CLR type name.</param>
        /// <returns></returns>
        [Obsolete("Use IsImplement(IClassDeclaration ...) instead.")]
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