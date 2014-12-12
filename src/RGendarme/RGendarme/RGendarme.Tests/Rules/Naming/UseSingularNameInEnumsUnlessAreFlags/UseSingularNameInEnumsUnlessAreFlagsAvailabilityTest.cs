using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Intentions.Test;
using JetBrains.ReSharper.Psi;
using NUnit.Framework;
using RGendarme.Rules.Naming.UseSingularNameInEnumsUnlessAreFlags;

namespace RGendarme.Tests.Rules.Naming.UseSingularNameInEnumsUnlessAreFlags
{
    [TestFixture]
    public class UseSingularNameInEnumsUnlessAreFlagsAvailabilityTest : QuickFixAvailabilityTestBase
    {
        protected override string RelativeTestDataPath
        {
            get { return @"Rules\Naming\UseSingularNameInEnumsUnlessAreFlags"; }
        }

        protected override bool HighlightingPredicate(IHighlighting highlighting, IPsiSourceFile psiSourceFile)
        {
            return highlighting is UseSingularNameInEnumsUnlessAreFlagsHighlighting;
        }

        [Test]
        public void Availability01()
        {
            DoTestFiles("availability01.cs");
        }
    }
}