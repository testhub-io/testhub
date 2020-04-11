using NUnit.Framework;
using System.Collections.Generic;
using TestHub.Data.DataModel;

namespace TestsHubUploadEndpoint.Tests.Integration
{
    [TestFixture(Category = "Integration")]
    public class DataLoaderTests
    {
        [Test]
        public void AddTestRun_test()
        {
            var dataContext = new TestHubDBContext(TestHubMocks.ConfigurationMock.Object);
            var org = new Organisation() { Name = "Integration-org" };
            dataContext.Entry(org);
            dataContext.SaveChanges();
            var dataLoader = new DataLoader(dataContext, "DataLoaderTests-Integration", org.Name);
            var testSuite = new TestSuite() { Name = "TestSuite1" };
            var testRun = new TestRun()
            {
                TestRunName = "Some test run",
                TestCases = new List<TestCase>() {
                new TestCase(){ Name = "Case 1", TestSuite = testSuite },
                new TestCase(){ Name = "Case 2",TestSuite = testSuite}
            }
            };
            dataLoader.Add(testRun);
            Assert.Pass();
        }


    }
}
