using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Text.Json;
using TestHub.Api.ApiDataProvider;
using TestHub.Data.DataModel;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;

namespace TestHub.Api.Tests.Integration
{

    [TestFixture, Explicit]
    public class DataProviderTests
    {
        private const string org = "Test-org";
        TestHubDBContext _db;
        [SetUp]
        public void SetUp()
        {
            var conf = new Mock<IConfiguration>(MockBehavior.Strict);
            var confSection = new Mock<IConfigurationSection>(MockBehavior.Strict);

            confSection.Setup(c => c["DefaultConnection"])
                .Returns("Host=localhost;Database=testHub;Username=root;Password=test_pass");

            // Amazon
            //confSection.Setup(c => c["DefaultConnection"])
            //    .Returns("Host=test-hub.chhksx9i82ny.us-east-2.rds.amazonaws.com;Database=testHub;Username=root;Password=test_pass");
            conf.Setup(c => c.GetSection("ConnectionStrings")).Returns(confSection.Object);
            _db = new TestHubDBContext(conf.Object);
        }

        [Test]
        public void GetProjectsTest()
        {
            var dataProvider = new DataProvider(_db, org, new UrlBuilder(Mock.Of<IUrlHelper>()));
            var results = dataProvider.GetProjects().Where(p => p.Name.Contains("many", StringComparison.OrdinalIgnoreCase));
            System.Diagnostics.Debug.WriteLine(JsonSerializer.Serialize(results));            
        }

        [Test]
        public void GetOrgSummaryTest()
        {
            var dataProvider = new DataProvider(_db, org, new UrlBuilder(Mock.Of<IUrlHelper>()));
            var results = dataProvider.GetOrgSummary();
            System.Diagnostics.Debug.WriteLine(JsonSerializer.Serialize(results));
        }

        [Test]
        public void GetTestRuns()
        {
            var dataProvider = new DataProvider(_db, org, new UrlBuilder(Mock.Of<IUrlHelper>()));
            var results = dataProvider.GetTestRuns("TestDataUpload-Regular");
            Assert.AreEqual(20, results.Count());
            System.Diagnostics.Debug.WriteLine(JsonSerializer.Serialize(results));
        }


        [Test]
        public void GetTestRunSummary()
        {        
            var dataProvider = new DataProvider(_db, org, new UrlBuilder(Mock.Of<IUrlHelper>()));
            var results = dataProvider.GetTestRunSummary("TestDataUpload-Regular", "22");            
            Assert.AreEqual(1907, results.TestCasesCount);            

            System.Diagnostics.Debug.WriteLine(JsonSerializer.Serialize(results));
        }

        [Test]
        public void GetTests()
        {
            var dataProvider = new DataProvider(_db, org, new UrlBuilder(Mock.Of<IUrlHelper>()));
            var results = dataProvider.GetTests("TestDataUpload-Regular", "22");
            Assert.AreEqual(1907, results.Tests.Count());            
            Assert.AreEqual(5, results.Tests.First().RecentResults.Count());

            System.Diagnostics.Debug.WriteLine(JsonSerializer.Serialize(results));
        }        

        [Test]
        public void GetTestResults()
        {
            var urlBuilder = getUrlBuilder();

            var dataProvider = new DataProvider(_db, org, urlBuilder);
            var results = dataProvider.GetTestResultsForProject("TestDataUpload-HugeReport");
            Assert.AreEqual(2, results.Data.Count());
            Assert.AreEqual(18476, results.Data.First().Passed);
            Assert.AreEqual(3, results.Data.First().Failed);
            Assert.AreEqual(0, results.Data.First().Skipped);
            //Assert.NotNull(results.Uri);

            System.Diagnostics.Debug.WriteLine(JsonSerializer.Serialize(results));
        }

        [Test]
        public void GetRunSummary()
        {
            var urlBuilder = getUrlBuilder();

            var dataProvider = new DataProvider(_db, org, urlBuilder);
            // Act 
            var results = dataProvider.GetTestRunSummary("TestDataUpload-Regular", "22");            

            // Assert
            Assert.AreEqual(1907, results.TestCasesCount);
            Assert.AreEqual(DateTime.Parse("2020-05-01 20:00:15.28127"), results.Timestamp);
            Assert.IsTrue (Math.Abs(238m - results.Time) < 1 );
            Assert.AreEqual(Data.TestResult.Failed, results.Status);
            Assert.AreEqual("22", results.Name);
            Assert.AreEqual(null, results.Coverage);
            Assert.AreEqual(1907, results.Summary.Passed);
            Assert.AreEqual(0, results.Summary.Failed);
            //Assert.NotNull("", results.Uri);

            System.Diagnostics.Debug.WriteLine(JsonSerializer.Serialize(results));
        }

        [Test]
        public void GetProjectCoverage()
        {
            var urlBuilder = getUrlBuilder();

            var dataProvider = new DataProvider(_db, "test-hub", urlBuilder);
            // Act 
            var results = dataProvider.GetCoverageHistory("testhub-api");

            // Assert
            Assert.Greater(results.Items.Count(), 37);
            Assert.AreEqual(3.716m, results.Items.First(c => c.TestRunName == "20200611.1").Coverage);
        }

        [Test]
        public void GetProjectSummary()
        {
            var urlBuilder = getUrlBuilder();

            var dataProvider = new DataProvider(_db, "test-hub", urlBuilder);
            // Act 
            var results = dataProvider.GetProjectSummary("testhub-api");

            // Assert
            Assert.Greater(results.TestsCount, 17);
            Assert.Greater(results.TestRunsCount, 37);
            Assert.AreEqual(3.716m, results.Coverage);
        }

        private static UrlBuilder getUrlBuilder()
        {
            var url = new Mock<IUrlHelper>();
            var urlBuilder = new UrlBuilder(url.Object);
            return urlBuilder;
        }
    }
}
