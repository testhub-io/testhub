using NUnit.Framework;

namespace TestHub.Api.Tests
{
    [TestFixture]
    public class UrlBulderTest
    {

        [Test]
        public void TestActionFailed()
        {
            Assert.Fail();        
        }

        [Test]
        public void TestActionPassed()
        {
            Assert.Fail();
        }

        [Test]
        [Ignore("Just for test")] 
        public void TestActionSkipped()
        {

        }
    }
}
