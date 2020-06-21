using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using TestHub.Data.DataModel;

namespace TestsHubUploadEndpoint.Tests.Integration
{
    [TestFixture(Category = "Integration")]
    public class DataUpload
    {
        TestHubDBContext _testHubDBContext;
        Organisation _org = new Organisation() { Name = "test-org" };
        static int counter = 1;

        [SetUp]
        public void Inti()
        {
            _testHubDBContext = new TestHubDBContext();            
            //_testHubDBContext = new TestHubDBContext("Host=test-hub.chhksx9i82ny.us-east-2.rds.amazonaws.com;Database=testHub;Username=root;Password=test_pass");            
        }

        [Test]
        public void FillInDb()
        {
            LoadJunitFile(TestData.GetFile("Kiln\\HugeNumberOfTests.xml"), "TestDataUpload-HugeReport");

            for (var i = 0; i < 20; i++)
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

        [Test]
        public void UploadAndMegeTwoJUnitFiles()
        {                                        
            var sw = new Stopwatch();
            sw.Start();
            var jUnitReader = new JUnitReader(
                new DataLoader(_testHubDBContext, "TestDataUpload-SmallJUnit", _org.Name));

            var testRun = $"{DateTime.Now.ToString("ddMMyyyy_hh:mm")}-{++counter}";
            var task = jUnitReader.Read(TestData.GetFile("Kiln\\Frontend-JUnit.xml"), testRun, "develop", "");
            var task2 = jUnitReader.Read(TestData.GetFile("test-results-rpclib.xml"), testRun, "develop", "");
            

            Task.WaitAll(task, task2);
            sw.Stop();

            Debug.WriteLine("Load time: " + sw.ElapsedMilliseconds);

            Assert.Pass();
        }

        private void LoadJunitFile(Stream stream, string projectName)
        {
            var sw = new Stopwatch();
            sw.Start();
            var jUnitReader = new JUnitReader(
                new DataLoader(_testHubDBContext, projectName, _org.Name));

            var task = jUnitReader.Read(stream, $"{DateTime.Now.ToString("ddMMyyyy")}-{++counter}", "develop", "");

            Task.WaitAll(task);
            sw.Stop();

            Debug.WriteLine("Load time: " + sw.ElapsedMilliseconds);
        }
    }
}
