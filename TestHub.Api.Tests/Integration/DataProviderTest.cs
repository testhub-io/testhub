using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var projects = dataProvider.GetProjects("Test-org");
            
        }
    }
}
