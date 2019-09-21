using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TestsHubUploadEndpoint.DataModel;

namespace TestsHubUploadEndpoint.Tests.Integration
{
    [TestFixture(Category = "Integration")]
    public class PostgressDataLoaderTests
    {
        [Test]
        public void AddTestRun_test()
        {
            var dataLoader = new PostgressDataLoader();
            dataLoader.Add(new TestRun() { TestRunName = "Some test run" });
            Assert.Pass();
        }
    }
}
