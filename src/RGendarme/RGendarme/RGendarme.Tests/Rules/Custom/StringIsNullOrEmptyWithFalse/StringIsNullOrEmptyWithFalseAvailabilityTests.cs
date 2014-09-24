using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Intentions.Test;
using JetBrains.ReSharper.Psi;
using NUnit.Framework;
using RGendarme.Rules.Custom.StringIsNullOrEmptyWithFalse;

namespace RGendarme.Tests.Rules.Custom.StringIsNullOrEmptyWithFalse
{
    [TestFixture]
    public class StringIsNullOrEmptyWithFalseAvailabilityTests : QuickFixAvailabilityTestBase
    {
        protected override string RelativeTestDataPath
        {
            get { return @"Rules\Custom\StringIsNullOrEmptyWithFalse"; }
        }

        protected override bool HighlightingPredicate(IHighlighting highlighting, IPsiSourceFile psiSourceFile)
        {
            return highlighting is StringIsNullOrEmptyWithFalseHighlight;
        }

        [Test]
        public void Availability01()
        {
            DoTestFiles("availability01.cs");
        }
    }
}