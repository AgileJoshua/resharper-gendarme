using JetBrains.ReSharper.Intentions.CSharp.Test;
using NUnit.Framework;
using RGendarme.Rules.Tutorial.ReverseString;

namespace RGendarme.Tests.Rules.Tutorial.ReverseString
{
    [TestFixture]
    public class ReverseStringExecuteTests : CSharpContextActionExecuteTestBase<ReverseStringAction>
    {
        protected override string ExtraPath
        {
            get { return @"Rules\Tutorial\ReverseStringAction"; }
        }

        protected override string RelativeTestDataPath
        {
            get { return @"Rules\Tutorial\ReverseStringAction"; }
        }

        [Test]
        public void ExecuteTest()
        {
            DoTestFiles("execute01.cs");
        }
    }
}