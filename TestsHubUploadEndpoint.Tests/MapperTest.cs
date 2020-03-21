using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TestHub.Data.DataModel;
using TestsHubUploadEndpoint.CoverageModel;
using report = TestsHubUploadEndpoint.ReportModel;

namespace TestsHubUploadEndpoint.Tests
{
    [TestFixture]
    public class MapperTest
    {

        [Test]
        [TestCase("Passes", TestResult.Passed)]
        [TestCase("PASS", TestResult.Passed)]
        [TestCase("passed", TestResult.Passed)]
        [TestCase("Failed", TestResult.Failed)]
        [TestCase("skipped", TestResult.Skipped)]
        public void TestCaseStatusMapping(
            string  status,
            TestResult result)
        {
            var mapper = MapperFactory.CreateMapper();
            var t = new report.TestCase()
            {
                Status = status
            };

            var dataCase = mapper.Map<TestCase>(t);
            Assert.AreEqual(result, dataCase.Status);

        }

        [Test]
        [TestCase("Passes", TestResult.Passed)]
        [TestCase("PASS", TestResult.Passed)]
        [TestCase("passed", TestResult.Passed)]
        [TestCase("Failed", TestResult.Failed)]
        [TestCase("skipped", TestResult.Skipped)]
        public void TestRunStatusMapping(
           string status,
           TestResult result)
        {
            var mapper = MapperFactory.CreateMapper();
            var t = new report.TestRun()
            {
                Status = status
            };

            var dataCase = mapper.Map<TestRun>(t);
            Assert.AreEqual(result, dataCase.Status);

        }
    }
}
