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

        [Test]
        public void QuickFix01()
        {
            DoTestFiles("quickfix01.cs.txt");
        }
    }
}