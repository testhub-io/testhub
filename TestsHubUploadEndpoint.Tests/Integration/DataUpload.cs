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
    public class DataUpload
    {
        TestHubDBContext _testHubDBContext;
        Organisation _org = new Organisation() { Name = "Test-org" };
        static int counter = 1;

        [SetUp]
        public void Inti()
        {            
            _testHubDBContext = new TestHubDBContext();
            _testHubDBContext.Add(_org);
        }

        [Test]
        public void FillInDb()
        {
            LoadJunitFile(TestData.GetFile("Kiln\\HugeNumberOfTests.xml"), "TestDataUpload-HugeReport");

            for (var i=0;i< 20; i++)
            {
                LoadJunitFile(TestData.GetFile("Kiln\\Frontend-JUnit.xml"), "TestDataUpload-Regular");
            }

            LoadJunitFile(TestData.GetFile("test-results-rpclib.xml"), "TestDataUpload-SmallJUnit");
            LoadJunitFile(TestData.GetFile("test-results-rpclib.xml"), "TestDataUpload-SmallJUnit");
        }

        [Test]
        public void UploadHugeJUnitFile()
        {
            LoadJunitFile(TestData.GetFile("Kiln\\HugeNumberOfTests.xml"), "TestDataUpload-HugeReport");
            Assert.Pass();
        }

        [Test]
        public void UploadSmallJUnitFile()
        {
            LoadJunitFile(TestData.GetFile("test-results-rpclib.xml"), "TestDataUpload-SmallJUnit");
            Assert.Pass();
        }

        [Test]
        public void UploadRegularJUnitFile()
        {
            LoadJunitFile(TestData.GetFile("Kiln\\Frontend-JUnit.xml"), "TestDataUpload - Regular");
            Assert.Pass();
        }

        private void LoadJunitFile(Stream stream, string projectName)
        {
            var sw = new Stopwatch();
            sw.Start();
            var jUnitReader = new JUnitReader(
                new DataLoader(new TestHubDBContext(), projectName, _org.Name));
            
            var task = jUnitReader.Read(stream, (++counter).ToString(), "develop", "");

            Task.WaitAll(task);
            sw.Stop();

            Debug.WriteLine("Load time: " + sw.ElapsedMilliseconds);
        }
    }
}
