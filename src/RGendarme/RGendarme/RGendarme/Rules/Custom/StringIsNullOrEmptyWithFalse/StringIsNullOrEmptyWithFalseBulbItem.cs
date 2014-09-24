using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.DocumentManagers;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.CodeStyle;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;

namespace RGendarme.Rules.Custom.StringIsNullOrEmptyWithFalse
{
    public class StringIsNullOrEmptyWithFalseBulbItem : IBulbAction
    {
        private readonly IEqualityExpression _expression;

        public StringIsNullOrEmptyWithFalseBulbItem(IEqualityExpression expression)
        {
            _expression = expression;
        }

        public void Execute(ISolution solution, ITextControl textControl)
        {
            if (!_expression.IsValid()) return;

            IFile containingFile = _expression.GetContainingFile();
            CSharpElementFactory elementFactory = CSharpElementFactory.GetInstance(_expression.GetPsiModule());

            IExpression newExpression = null;
            _expression.GetPsiServices().Transactions.Execute(GetType().Name, () =>
            {
                using (solution.GetComponent<IShellLocks>().UsingWriteLock())
                {
                    newExpression = ModificationUtil.ReplaceChild(
                        _expression, elementFactory.CreateExpression(GetReplaceTemplate(_expression)));
                }
            });

            if (newExpression != null)
            {
                IRangeMarker marker = newExpression.GetDocumentRange().CreateRangeMarker(solution.GetComponent<DocumentManager>());

                containingFile.OptimizeImportsAndRefs(marker, false, true, NullProgressIndicator.Instance);
            }
        }

        private string GetReplaceTemplate(IEqualityExpression expression)
        {
            string outputTemplate = string.Empty;

            var left = expression.LeftOperand as IInvocationExpression;
            if (left != null)
            {
                outputTemplate = "!" + left.GetText();
            }

            return outputTemplate;
        }

        public string Text { get { return "Remove redundant false"; } }
    }
}