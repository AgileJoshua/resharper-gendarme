using JetBrains.ProjectModel;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.TextControl;

namespace RGendarme.Rules.Design.AvoidVisibleFields
{
    public class AvoidVisibleFieldsCreatePropertyBulbItem : IBulbAction
    {
        private readonly IMultipleFieldDeclaration _declaration;
        public AvoidVisibleFieldsCreatePropertyBulbItem(IMultipleFieldDeclaration declaration)
        {
            _declaration = declaration;
        }

        public void Execute(ISolution solution, ITextControl textControl)
        {
            throw new System.NotImplementedException();
        }

        public string Text { get { return "Convert to property"; } }
    }
}