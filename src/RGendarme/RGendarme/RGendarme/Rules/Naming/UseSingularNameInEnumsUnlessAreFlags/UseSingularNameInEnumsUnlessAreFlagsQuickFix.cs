using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;
using RGendarme.Lib;

namespace RGendarme.Rules.Naming.UseSingularNameInEnumsUnlessAreFlags
{
    [QuickFix]
    public class UseSingularNameInEnumsUnlessAreFlagsQuickFix : IQuickFix
    {
        private readonly UseSingularNameInEnumsUnlessAreFlagsHighlighting _highlighting;
        public UseSingularNameInEnumsUnlessAreFlagsQuickFix([NotNull]UseSingularNameInEnumsUnlessAreFlagsHighlighting highlighting)
        {
            _highlighting = highlighting;
        }

        public IEnumerable<IntentionAction> CreateBulbItems()
        {
            return new UseSingularNameInEnumsUnlessAreFlagsBulbItem(_highlighting.Declaration).ToQuickFixAction();
        }

        public bool IsAvailable(IUserDataHolder cache)
        {
            return _highlighting.IsValid();
        }
    }

    public class UseSingularNameInEnumsUnlessAreFlagsBulbItem : IBulbAction
    {
        private readonly IEnumDeclaration _declaration;
        public UseSingularNameInEnumsUnlessAreFlagsBulbItem(IEnumDeclaration declaration)
        {
            _declaration = declaration;
        }

        public void Execute(ISolution solution, ITextControl textControl)
        {
            if (!_declaration.IsValid()) return;

            CSharpElementFactory elementFactory = CSharpElementFactory.GetInstance(_declaration.GetPsiModule());

            ITypeElement attributeTypeElement;
            if (TypeHelper.TryGetTypeElement(typeof(System.FlagsAttribute).FullName, _declaration.GetPsiModule(), _declaration.GetResolveContext(), out attributeTypeElement))
            {
                IAttribute flagsAttribute = elementFactory.CreateAttribute(attributeTypeElement);

                _declaration.GetPsiServices().Transactions.Execute(GetType().Name, () =>
                {
                    using (solution.GetComponent<IShellLocks>().UsingWriteLock())
                    {
                        _declaration.AddAttributeAfter(flagsAttribute, null);
                    }
                });
            }
        }

        public string Text { get { return "Add Flags attribute."; } }
    }
}