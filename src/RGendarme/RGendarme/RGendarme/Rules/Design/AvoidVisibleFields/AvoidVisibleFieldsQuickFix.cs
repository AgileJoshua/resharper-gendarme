using System.Collections.Generic;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.Util;

namespace RGendarme.Rules.Design.AvoidVisibleFields
{
    [QuickFix]
    public class AvoidVisibleFieldsQuickFix : IQuickFix
    {
        private readonly AvoidVisibleFieldsHighlighting _highlighting;
        public AvoidVisibleFieldsQuickFix(AvoidVisibleFieldsHighlighting highlighting)
        {
            _highlighting = highlighting;
        }

        public IEnumerable<IntentionAction> CreateBulbItems()
        {
            var bulbs = new List<IntentionAction>();

            bulbs.AddRange(new AvoidVisibleFieldsCreatePropertyBulbItem(_highlighting.FieldDeclaration).ToQuickFixAction());

            bulbs.AddRange(new AvoidVisibleFieldsCreateMethodBulbItem(_highlighting.FieldDeclaration).ToQuickFixAction());

            return bulbs;
        }

        public bool IsAvailable(IUserDataHolder cache)
        {
            return _highlighting.IsValid();
        }
    }
}