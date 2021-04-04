using Moq;
using NUnit.Framework;
using TestsHubUploadEndpoint.Coverage;


namespace TestsHubUploadEndpoint.Tests
{
    [TestFixture]
    public class CoberturaReaderTest
    {
        [Test]
        public void TestReader()
        {
            const string TestRunName = "test-test-run";
            var dl = new Mock<IDataLoader>(MockBehavior.Strict);
            dl.Setup(d => d.Add(It.Is<CoverageSummary>(c => c.LinesValid == 938 && c.LinesCovered == 63 && c.TestRunName == TestRunName)))
                .Verifiable();

            var coberturaReader = new CoberturaReader(dl.Object);

            var coverageStream = TestData.GetCoverageReport("cobertura", "cobertura-coverage.xml");
            
            coberturaReader.Read(coverageStream, TestRunName);
            dl.VerifyAll();
        }
    }
}
