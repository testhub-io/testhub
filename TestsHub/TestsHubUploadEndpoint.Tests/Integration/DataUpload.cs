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

        private static void LoadJunitFile(Stream stream, string projectName)
        {
            var sw = new Stopwatch();
            sw.Start();
            var jUnitReader = new JUnitReader(
                new DataLoader(new TestHubDBContext(), projectName));
            
            var task = jUnitReader.Read( stream);
            Task.WaitAll(task);
            sw.Stop();

            Debug.WriteLine("Load time: " + sw.ElapsedMilliseconds);
        }
    }
}
