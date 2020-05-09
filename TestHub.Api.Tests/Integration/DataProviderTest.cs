﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Text.Json;
using TestHub.Api.ApiDataProvider;
using TestHub.Data.DataModel;
using Microsoft.Extensions.Configuration;
using System.Linq;

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
            Assert.AreEqual(11, results.Count());
            System.Diagnostics.Debug.WriteLine(JsonSerializer.Serialize(results));
        }


        [Test]
        public void GetTestRun()
        {        
            var dataProvider = new DataProvider(_db, org, new UrlBuilder(Mock.Of<IUrlHelper>()));
            var results = dataProvider.GetTestRun("testhub-api", "local_run");
            Assert.AreEqual(11, results.TestCases.Count());
            Assert.AreEqual(11, results.TestCasesCount);
            System.Diagnostics.Debug.WriteLine(JsonSerializer.Serialize(results));
        }

        [Test]
        public void GetTestResults()
        {
            var dataProvider = new DataProvider(_db, org, new UrlBuilder(Mock.Of<IUrlHelper>()));
            var results = dataProvider.GetTestResultsForProject("TestDataUpload-HugeReport");
            Assert.AreEqual(2, results.Data.Count());
            Assert.AreEqual(18476, results.Data.First().Passed  );
            Assert.AreEqual(3, results.Data.First().Failed);
            Assert.AreEqual(0, results.Data.First().Skipped);

            System.Diagnostics.Debug.WriteLine(JsonSerializer.Serialize(results));
        }             

    }
}
