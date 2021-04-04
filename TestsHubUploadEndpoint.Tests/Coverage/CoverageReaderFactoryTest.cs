using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Shouldly;
using TestsHubUploadEndpoint.Coverage;

namespace TestsHubUploadEndpoint.Tests.Coverage
{
    [TestFixture]
    public class CoverageReaderFactoryTest
    {
        [Test]
        public void CreateReader_ReturnsJacocoReader()
        {
            var factory = new CoverageReaderFactory();

            const string TestRunName = "test-test-run";
            var dl = new Mock<IDataLoader>(MockBehavior.Strict);
            dl.Setup(d => d.Add(It.Is<CoverageSummary>(c => c.LinesValid == 6 && c.LinesCovered == 5 && c.TestRunName == TestRunName)))
                .Verifiable();

            using (var coverageStream = TestData.GetCoverageReport("jacoco", "jacoco.xml"))
            {
                var reader = factory.CreateReader(coverageStream, dl.Object);
                reader.Read(coverageStream, TestRunName);
                dl.VerifyAll();
            }                
        }

        [Test]
        public void CreateReader_ReturnsCoberturaReader()
        {
            var factory = new CoverageReaderFactory();
            var dl = new Mock<IDataLoader>(MockBehavior.Strict);

            using (var coverageStream = TestData.GetCoverageReport("cobertura", "cobertura-coverage.xml"))
            {
                var reader = factory.CreateReader(coverageStream, dl.Object);
                reader.ShouldBeOfType<CoberturaReader>();
            }
        }
    }
}
