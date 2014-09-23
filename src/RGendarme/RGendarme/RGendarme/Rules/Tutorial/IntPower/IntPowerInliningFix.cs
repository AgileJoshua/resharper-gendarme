using System;
using System.Linq;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace RGendarme.Rules.Tutorial.IntPower
{
    [QuickFix]
    public class IntPowerInliningFix : QuickFixBase
    {
        private readonly IntPowerHighlighting highlighting;

        public IntPowerInliningFix(IntPowerHighlighting highlighting)
        {
            this.highlighting = highlighting;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var arg = highlighting.Expression.Arguments[0];
            var factory = CSharpElementFactory.GetInstance(highlighting.Expression.GetPsiModule());
            var replacement = factory.CreateExpression(
              Enumerable.Range(0, highlighting.Power).Select(i => "$0").Join("*"), arg.Value);

            ModificationUtil.ReplaceChild(highlighting.Expression, replacement);
            return null;
        }

        public override string Text
        {
            get { return "Inline integer power"; }
        }

        public override bool IsAvailable(IUserDataHolder cache)
        {
            int power = highlighting.Power;
            return power == 2 || power == 3;
        }
    }
}