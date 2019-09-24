using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using TestsHub.Data.DataModel;
using TestsHubUploadEndpoint.Tests.TestData;

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

        [Test]
        public void JustUpload()
        {
            var dataLoader = new PostgressDataLoader();
            var sw = new Stopwatch();
            sw.Start();
            var jUnitReader = new JUnitReader(new PostgressDataLoader());
            var task = jUnitReader.Read(TestData.TestData.GetFile("Kiln\\HugeNumberOfTests.xml"));
            Task.WaitAll(task);
            sw.Stop();

            System.Diagnostics.Debug.WriteLine("Load time: " + sw.ElapsedMilliseconds);
            Assert.Pass();
        }
    }
}
