using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace RGendarme.Rules.Naming.UseCorrectSuffix
{
    [ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(UseCorrectSuffixHighlighting) })]
    public class UseCorrectSuffixAnalyzer : ElementProblemAnalyzer<IClassDeclaration>
    {
        protected override void Run(IClassDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            // key - name, value - base type
            var dict = new Dictionary<string, string>
            {
                {"Attribute", "System.Attribute"},
                {"EventArgs", "System.EventArgs"},
                {"Exception", "System.Exception"},
                {"DataSet", "System.Data.DataSet"},
                {"Stream", "System.IO.Stream"},
                {"Permission", "System.Security.IPermission"},
                {"Condition", "System.Security.Policy.IMembershipCondition"}
            };

            foreach (var kvp in dict)
            {
                Analyze(element, consumer, kvp.Key, kvp.Value, (id, suffix) => new UseCorrectSuffixHighlighting(id, suffix));
            }

            // special cases
            // dictionary
            AnalyzeWrongClassName(element, consumer, "Dictionary", "System.Collections.IDictionary", (id, suffix) => new UseCorrectSuffixHighlighting(id, suffix));
            AnalyzeWrongClassName(element, consumer, "Dictionary", "System.Collections.Generic.IDictionary", (id, suffix) => new UseCorrectSuffixHighlighting(id, suffix));

            // collections
            AnalyzeWrongClassName(element, consumer, "Collection", "System.Collections.ICollection", (id, suffix) => new UseCorrectSuffixHighlighting(id, suffix));
            AnalyzeWrongClassName(element, consumer, "Collection", "System.Collections.IEnumerable", (id, suffix) => new UseCorrectSuffixHighlighting(id, suffix));
            AnalyzeWrongClassName(element, consumer, "Collection", "System.Collections.Generic.ICollection", (id, suffix) => new UseCorrectSuffixHighlighting(id, suffix));
            AnalyzeWrongClassName(element, consumer, "Collection", "System.Collections.Generic.IEnumerable", (id, suffix) => new UseCorrectSuffixHighlighting(id, suffix));
        }

        private void Analyze(IClassDeclaration element, IHighlightingConsumer consumer, string suffix, string baseClass,
            Func<ICSharpIdentifier, string, IHighlighting> highlighting)
        {
            AnalyzeWrongClassName(element, consumer, suffix, baseClass, highlighting);
            AnalyzeNotImplement(element, consumer, suffix, baseClass, highlighting);
        }

        private void AnalyzeWrongClassName(IClassDeclaration element, IHighlightingConsumer consumer, string suffix, string baseClass, Func<ICSharpIdentifier, string, IHighlighting> highlighting)
        {
            string errorMsg = string.Format("Class doesn't have {0} suffix", suffix);

            // 1. get exntend list
            IExtendsList extends = element.ExtendsList;
            if (extends == null)
                return;

            // 2. check if class extends Attribute class, it has to have Attbite suffix
            if (!IsImplement(extends, baseClass))
                return;

            // 3. get class name
            string name = element.NameIdentifier.Name;
            if (!string.IsNullOrEmpty(name) && !name.EndsWith(suffix))
            {
                ICSharpIdentifier identifier = element.NameIdentifier;
                consumer.AddHighlighting(highlighting(identifier, errorMsg), identifier.GetDocumentRange(), identifier.GetContainingFile());
            }
        }

        private void AnalyzeNotImplement(IClassDeclaration element, IHighlightingConsumer consumer, string suffix, string baseClass,
            Func<ICSharpIdentifier, string, IHighlighting> highlighting)
        {
            string errorMsg = string.Format("Has {0} suffix but doesn't extend {1} class", suffix, baseClass); 

            // 1. get class name
            string name = element.NameIdentifier.Name;

            if (!string.IsNullOrEmpty(name) && !name.EndsWith(suffix))
                return;

            // 2. get exntend list
            IExtendsList extends = element.ExtendsList;
            if (extends == null)
            {
                ICSharpIdentifier nameIdentifier = element.NameIdentifier;
                consumer.AddHighlighting(highlighting(nameIdentifier, errorMsg), nameIdentifier.GetDocumentRange(), nameIdentifier.GetContainingFile());
                return;
            }

            // 3. check if class has Attribute suffix but doesn't extend Attribute class
            if (!IsImplement(extends, baseClass))
            {
                ICSharpIdentifier nameIdentifier = element.NameIdentifier;
                consumer.AddHighlighting(highlighting(nameIdentifier, errorMsg), nameIdentifier.GetDocumentRange(), nameIdentifier.GetContainingFile());
            }
        }

        /// <summary>
        /// Check if extend list contains specific type.
        /// </summary>
        /// <param name="extends">Extend list</param>
        /// <param name="baseClass">Full CLR type name.</param>
        /// <returns></returns>
        private bool IsImplement(IExtendsList extends, string baseClass)
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
    }
}