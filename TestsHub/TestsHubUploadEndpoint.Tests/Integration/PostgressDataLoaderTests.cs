using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TestsHub.Data.DataModel;

namespace TestsHubUploadEndpoint.Tests.Integration
{
    [TestFixture(Category = "Integration")]
    public class PostgressDataLoaderTests
    {
        [Test]
        public void AddTestRun_test()
        {
            var dataLoader = new PostgressDataLoader();
            var testRun = new TestRun() { TestRunName = "Some test run", TestCases = new List<TestCase>() {
                new TestCase(){ Name = "Case 1"},
                new TestCase(){ Name = "Case 2"}
            } };
            dataLoader.Add(testRun);
            Assert.Pass();
        }
    }
}
