using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.DocumentManagers;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.CodeStyle;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Css.Impl.Validation.Descriptions;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.UI.Resources;
using JetBrains.Util;

namespace RGendarme.Rules.Tutorial.Custom
{
    [ElementProblemAnalyzer(new[] { typeof(IEqualityExpression) }, HighlightingTypes = new[] { typeof(StringIsNullOrEmptyWithFalseHighlight) })]
    public class StringIsNullOrEmptyWithFalseAnalyzer : ElementProblemAnalyzer<IEqualityExpression>
    {
        protected override void Run(IEqualityExpression element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            bool isNullOrEmptyFunc = false;
            var left = element.LeftOperand as IInvocationExpression;
            if (left != null)
            {
                var r = left.InvocationExpressionReference.Resolve();
                var method = r.DeclaredElement as IMethod;
                if (method != null)
                {
                    var parent = method.GetContainingType();
                    if (parent != null)
                    {
                        isNullOrEmptyFunc = parent.GetClrName().FullName.Equals("System.String")
                                            && method.ShortName.Equals("IsNullOrEmpty");
                    }
                }
            }

            bool isFalseConstant = false;
            var right = element.RightOperand as ICSharpLiteralExpression;
            if (right != null && right.IsConstantValue())
            {
                ConstantValue cv = right.ConstantValue;
                if (cv.IsBoolean())
                {
                    var value = (bool) cv.Value;
                    isFalseConstant = !value;
                }
            }

            if (isNullOrEmptyFunc && isFalseConstant)
                consumer.AddHighlighting(new StringIsNullOrEmptyWithFalseHighlight(element), element.GetDocumentRange(), element.GetContainingFile());
        }
    }

    [StaticSeverityHighlighting(Severity.WARNING, CSharpLanguage.Name)]
    public class StringIsNullOrEmptyWithFalseHighlight : IHighlighting
    {
        public IEqualityExpression Expression { get; private set; }

        public StringIsNullOrEmptyWithFalseHighlight(IEqualityExpression expression)
        {
            Expression = expression;
        }

        public bool IsValid()
        {
            return Expression != null && Expression.IsValid();
        }

        public string ToolTip { get { return "Redundant use of false constant"; } }
        public string ErrorStripeToolTip { get { return ToolTip; } }
        public int NavigationOffsetPatch {get { return 0; } }
    }

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

    [QuickFix]
    public class StringIsNullOrEmptyWithFalseQuickFix : IQuickFix
    {
        private readonly StringIsNullOrEmptyWithFalseHighlight _highlight;

        public StringIsNullOrEmptyWithFalseQuickFix([NotNull] StringIsNullOrEmptyWithFalseHighlight highlight)
        {
            _highlight = highlight;
        }

        public IEnumerable<IntentionAction> CreateBulbItems()
        {
            return new StringIsNullOrEmptyWithFalseBulbItem(_highlight.Expression).ToQuickFixAction();
        }

        public bool IsAvailable(IUserDataHolder cache)
        {
            return _highlight.IsValid();
        }
    }
}