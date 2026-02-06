using NUnit.Framework;
using TestHub.Api.Authentication;
using FluentAssertions;

namespace TestHub.Api.Tests
{
    [TestFixture]
    public class ApiKeyValidatorTest
    {
        [Test]
        public void GenerateApiKey_ShouldReturnNonEmptyString()
        {
            var apiKey = ApiKeyValidator.GenerateApiKey("TestOrg");

            apiKey.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void GenerateApiKey_ShouldReturnConsistentKeyForSameOrg()
        {
            var apiKey1 = ApiKeyValidator.GenerateApiKey("TestOrg");
            var apiKey2 = ApiKeyValidator.GenerateApiKey("TestOrg");

            apiKey1.Should().Be(apiKey2);
        }

        [Test]
        public void GenerateApiKey_ShouldReturnDifferentKeysForDifferentOrgs()
        {
            var apiKey1 = ApiKeyValidator.GenerateApiKey("OrgA");
            var apiKey2 = ApiKeyValidator.GenerateApiKey("OrgB");

            apiKey1.Should().NotBe(apiKey2);
        }

        [Test]
        public void GenerateApiKey_ShouldBeCaseInsensitive()
        {
            var apiKey1 = ApiKeyValidator.GenerateApiKey("TestOrg");
            var apiKey2 = ApiKeyValidator.GenerateApiKey("TESTORG");
            var apiKey3 = ApiKeyValidator.GenerateApiKey("testorg");

            apiKey1.Should().Be(apiKey2);
            apiKey1.Should().Be(apiKey3);
        }

        [Test]
        public void GenerateApiKey_ShouldReturnHexString()
        {
            var apiKey = ApiKeyValidator.GenerateApiKey("TestOrg");

            apiKey.Should().MatchRegex("^[a-f0-9]+$");
        }

        [Test]
        public void GenerateApiKey_ShouldReturnExpectedLength()
        {
            var apiKey = ApiKeyValidator.GenerateApiKey("TestOrg");

            // SHA1 produces 20 bytes, which is 40 hex characters
            apiKey.Should().HaveLength(40);
        }

        [Test]
        public void IsKeyValid_ShouldReturnTrueForValidKey()
        {
            var org = "TestOrg";
            var apiKey = ApiKeyValidator.GenerateApiKey(org);

            var result = ApiKeyValidator.IsKeyValid(apiKey, org);

            result.Should().BeTrue();
        }

        [Test]
        public void IsKeyValid_ShouldReturnFalseForInvalidKey()
        {
            var org = "TestOrg";
            var invalidKey = "invalidkey123";

            var result = ApiKeyValidator.IsKeyValid(invalidKey, org);

            result.Should().BeFalse();
        }

        [Test]
        public void IsKeyValid_ShouldReturnFalseForMismatchedOrg()
        {
            var org1 = "OrgA";
            var org2 = "OrgB";
            var apiKey = ApiKeyValidator.GenerateApiKey(org1);

            var result = ApiKeyValidator.IsKeyValid(apiKey, org2);

            result.Should().BeFalse();
        }

        [Test]
        public void IsKeyValid_ShouldBeCaseInsensitiveForOrg()
        {
            var apiKey = ApiKeyValidator.GenerateApiKey("TestOrg");

            var result1 = ApiKeyValidator.IsKeyValid(apiKey, "TestOrg");
            var result2 = ApiKeyValidator.IsKeyValid(apiKey, "TESTORG");
            var result3 = ApiKeyValidator.IsKeyValid(apiKey, "testorg");

            result1.Should().BeTrue();
            result2.Should().BeTrue();
            result3.Should().BeTrue();
        }

        [Test]
        public void IsKeyValid_ShouldReturnFalseForEmptyKey()
        {
            var result = ApiKeyValidator.IsKeyValid("", "TestOrg");

            result.Should().BeFalse();
        }
    }
}
