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
            Assert.IsTrue(ApiKeyValidator.IsKeyValid("2b8e743efdefa277d6563e06f92c1574d8da3f82", "some_org"));

        }
    }
}
