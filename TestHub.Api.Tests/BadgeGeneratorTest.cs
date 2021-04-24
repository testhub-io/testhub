using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using TestHub.Api.Badge;
using FluentAssertions;

namespace TestHub.Api.Tests
{
    [TestFixture]
    public class BadgeGeneratorTest
    {
        [Test]
        public void GenerateBadgeFailingTests()
        {
            var svg = BadgeGenerator.GenerateBadge(10, 2,  95M);
            svg.Should().Contain("2 out of 10 tests failing | coverage: 95%");
            svg.Should().Contain("e05d44");            
        }

        [Test]
        public void GenerateRedBadgeWithoutCoverage()
        {
            var svg = BadgeGenerator.GenerateBadge(10, 2, null);
            svg.Should().Contain("2 out of 10 tests failing");
            svg.Should().Contain("e05d44");            
        }

        [Test]
        public void GenerateGreenBadgeWithoutCoverage()
        {
            var svg = BadgeGenerator.GenerateBadge(10, 0, null);
            svg.Should().Contain("all 10 tests passing");
            svg.Should().Contain("97CA00");            
        }

        [Test]
        public void GenerateGreenBadgeWithoutData()
        {
            var svg = BadgeGenerator.GenerateBadge(null, 0, null);
            svg.Should().Contain("no tests");
            svg.Should().Contain("9f9f9f");            
        }
    }
}
