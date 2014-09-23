using JetBrains.ReSharper.Intentions.Test;
using JetBrains.ReSharper.IntentionsTests;
using NUnit.Framework;
using RGendarme.Rules.Tutorial.IntPower;

namespace RGendarme.Tests.Rules.Tutorial.IntPower
{
    [TestFixture]
//    public class IntPowQuickFixTests : QuickFixAvailabilityTestBase
    public class IntPowQuickFixTests : QuickFixTestBase<IntPowerInliningFix>
    {
        protected override string RelativeTestDataPath
        {
            get { return @"Rules\Tutorial\IntPower"; }
        }

//        protected override bool HighlightingPredicate(JetBrains.ReSharper.Daemon.IHighlighting highlighting, JetBrains.ReSharper.Psi.IPsiSourceFile psiSourceFile)
//        {
//            return highlighting is IntPowerHighlighting;
//        }

        [Test]
        public void QuickFix01()
        {
            DoTestFiles("quickfix01.cs.txt");
        }
    }

    [TestFixture]
    public class IntPowQuickFixAvailabilityTests : QuickFixAvailabilityTestBase
    {
        protected override string RelativeTestDataPath
        {
            get { return @"Rules\Tutorial\IntPower"; }
        }

        protected override bool HighlightingPredicate(JetBrains.ReSharper.Daemon.IHighlighting highlighting, JetBrains.ReSharper.Psi.IPsiSourceFile psiSourceFile)
        {
            return highlighting is IntPowerHighlighting;
        }

        [Test]
        public void Availability01()
        {
            DoTestFiles("availability01.cs");
        }
    }
}