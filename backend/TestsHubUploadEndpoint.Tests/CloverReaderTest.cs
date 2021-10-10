using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TestsHubUploadEndpoint.Coverage;

namespace TestsHubUploadEndpoint.Tests
{
    class CloverReaderTest
    {
        [Test]
        public void TestReader()
        {
            const string TestRunName = "test-test-run";
            var dl = new Mock<IDataLoader>(MockBehavior.Strict);
            dl.Setup(d => d.Add(It.Is<CoverageSummary>(c => c.LinesValid == 1342 && c.LinesCovered == 860 && c.TestRunName == TestRunName)))
                .Verifiable();

            var cloverReader = new CloverReader(dl.Object);
            using (var coverageStream = TestData.GetCoverageReport("clover", "clover.xml"))
            {
                cloverReader.Read(coverageStream, TestRunName);
                dl.VerifyAll();
            }
        }
    }
}
