using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TestsHub.Data.DataModel;

namespace TestsHubUploadEndpoint.Tests.Integration
{
    [TestFixture(Category = "Integration")]
    public class DataLoaderTests
    {
        [Test]
        public void AddTestRun_test()
        {
            var dataLoader = new DataLoader(new TestHubDBContext(), "DataLoaderTests-Integration");
            var testRun = new TestRun() { TestRunName = "Some test run", TestCases = new List<TestCase>() {
                new TestCase(){ Name = "Case 1"},
                new TestCase(){ Name = "Case 2"}
            } };
            dataLoader.Add(testRun);
            Assert.Pass();
        }      
    }
}
