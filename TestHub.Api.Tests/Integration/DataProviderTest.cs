using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Text.Json;
using TestHub.Api.ApiDataProvider;
using TestHub.Data.DataModel;

namespace TestHub.Api.Tests.Integration
{

    [TestFixture, Explicit]
    public class DataProviderTests
    {
        [Test]
        public void GetProjectsTest()
        {
            var dataProvider = new DataProvider(new TestHubDBContext(), "Test-org", new UrlBuilder(Mock.Of<IUrlHelper>()));
            var results = dataProvider.GetProjects();
            System.Diagnostics.Debug.WriteLine(JsonSerializer.Serialize(results));
            Assert.Pass();
        }

        [Test]
        public void GetTestRuns()
        {
            var dataProvider = new DataProvider(new TestHubDBContext(), "Test-org", new UrlBuilder(Mock.Of<IUrlHelper>()));
            var results = dataProvider.GetTestRuns("dotNet","12");
            System.Diagnostics.Debug.WriteLine(JsonSerializer.Serialize(results));
        }
    }
}
