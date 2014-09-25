using JetBrains.ProjectModel;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.TextControl;

namespace RGendarme.Rules.Design.AvoidVisibleFields
{
    public class AvoidVisibleFieldsCreateMethodBulbItem : IBulbAction
    {
        private readonly IMultipleFieldDeclaration _declaration;

        public AvoidVisibleFieldsCreateMethodBulbItem(IMultipleFieldDeclaration declaration)
        {
            _declaration = declaration;
        }

        public void Execute(ISolution solution, ITextControl textControl)
        {
            throw new System.NotImplementedException();
        }

        public string Text { get { return "Convert to method"; } }
    }
}