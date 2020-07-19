using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TestsHubUploadEndpoint.CoverageModel;

namespace TestsHubUploadEndpoint.Tests
{
    [TestFixture]
    public class JacocoReaderTest
    {
        [Test]
        public void TestReader()
        {
            const string TestRunName = "test-test-run";
            var dl = new Mock<IDataLoader>(MockBehavior.Strict);
            dl.Setup(d => d.Add(It.Is<CoverageSummary>(c => c.LinesValid == 6 && c.LinesCovered == 5 && c.TestRunName == TestRunName)))
                .Verifiable();

            var jacocoReader = new JacocoReader(dl.Object);
            var coverageStream = TestData.GetFile("coverage/jacoco.xml");

            jacocoReader.Read(coverageStream, TestRunName);
            dl.VerifyAll();
        }

    }

}
