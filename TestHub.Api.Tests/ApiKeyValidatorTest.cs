using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TestHub.Api.Authentication;

namespace TestHub.Api.Tests
{
    [TestFixture]
    public class ApiKeyValidatorTest
    {
        [Test]
        public void IsKeyValidTest()
        {
            Assert.IsTrue(ApiKeyValidator.IsKeyValid("", "test-org"));

        }
    }
}
