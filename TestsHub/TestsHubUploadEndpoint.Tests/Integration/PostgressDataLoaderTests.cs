﻿using NUnit.Framework;
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
            var dataContext = new TestHubDBContext();
            var org = new Organisation() { Name = "Integration-org" };
            dataContext.Entry(org);
            dataContext.SaveChanges();
            var dataLoader = new DataLoader(dataContext, "DataLoaderTests-Integration", org.Name);
            var testRun = new TestRun() { TestRunName = "Some test run", TestCases = new List<TestCase>() {
                new TestCase(){ Name = "Case 1"},
                new TestCase(){ Name = "Case 2"}
            } };
            dataLoader.Add(testRun);
            Assert.Pass();
        }      
    }
}
