using JetBrains.ReSharper.Intentions.CSharp.Test;
using NUnit.Framework;
using RGendarme.Rules.Tutorial.ReverseString;

namespace RGendarme.Tests.Rules.Tutorial.ReverseString
{
    [TestFixture]
    public class ReverseStringAvailabilityTests : CSharpContextActionAvailabilityTestBase<ReverseStringAction>
    {
        [Test]
        public void AvailabilityTest()
        {
            DoTestFiles("availability01.cs");
        }

        protected override string ExtraPath
        {
            get { return @"Rules\Tutorial\ReverseStringAction"; }
        }

        protected override string RelativeTestDataPath
        {
            get { return @"Rules\Tutorial\ReverseStringAction"; }
        }
    }
}