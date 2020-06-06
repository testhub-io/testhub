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
            conf.Setup(c => c.GetSection("ConnectionStrings")).Returns(confSection.Object);
            _db = new TestHubDBContext(conf.Object);
        }

        [Test]
        public void GetProjectsTest()
        {
            var dataProvider = new DataProvider(_db, org, new UrlBuilder(Mock.Of<IUrlHelper>()));
            var results = dataProvider.GetProjects();
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
            var results = dataProvider.GetTestRuns("dotNet");
            Assert.AreEqual(2, results.Count());
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

        private static UrlBuilder getUrlBuilder()
        {
            var url = new Mock<IUrlHelper>();
            var urlBuilder = new UrlBuilder(url.Object);
            return urlBuilder;
        }
    }
}
