﻿using JetBrains.ReSharper.Intentions.CSharp.Test;
using NUnit.Framework;
using RGendarme.Rules.Tutorial.ReverseString;

namespace RGendarme.Tests
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
            get { return "ReverseStringAction"; }
        }

        protected override string RelativeTestDataPath
        {
            get { return "ReverseStringAction"; }
        }
    }
}