using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.Util;

namespace RGendarme.Rules.Custom.StringIsNullOrEmptyWithFalse
{
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