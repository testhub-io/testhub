using Moq;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestsHubUploadEndpoint;
using TestsHubUploadEndpoint.ReportModel;
using TestsHubUploadEndpoint.Tests;

namespace Tests
{
    public class JUnitReaderTests
    {
        JUnitReader _reader;
        List<TestCase> _testCasesReported;
        TestRun _testRunReported;

        [SetUp]
        public void Setup()
        {
            var dataLoaderMock = new Mock<IDataLoader>();
            _testRunReported = new TestRun();
            _testCasesReported = new List<TestCase>();
            dataLoaderMock.Setup(s => s.Add(It.IsAny<TestRun>(), It.IsAny<IEnumerable<TestCase>>()))
                .Callback<TestRun, IEnumerable<TestCase>>((t, c) =>
                {
                    _testRunReported = t;
                    _testCasesReported = c.ToList();
                });


            _reader = new JUnitReader(dataLoaderMock.Object);
        }

        [Test]
        public void Test_Read()
        {
            // Arrange             
            var xmlReader = TestData.GetFile("MinimalJUnit.xml");

            // Act 
            Task.WaitAll(_reader.Read(xmlReader, "tr1", "develop", ""));

            // Assert
            _testCasesReported.Count.ShouldBe(2);

            var case1 = _testCasesReported.First();
            case1.Name.ShouldBe("PassedTest");
            case1.ClassName.ShouldBe("aspnetappDependency.Tests.UnitTest1");
            case1.Status.ShouldBe("passed");
            case1.Time.ShouldBe(0.0000749m);
            case1.TestSuite.Name.ShouldBe("aspnetappDependency.Tests.UnitTest1");
            _testRunReported.TestCasesCount.ShouldBe(2);
        }

        [Test]
        public void Test_ReadFileWithFailure()
        {
            // Arrange 
            var xmlReader = TestData.GetFile("failure.xml");

            // Act 
            Task.WaitAll(_reader.Read(xmlReader, "tr1", "develop", ""));

            // Assert
            var faileDetstOutput = _testCasesReported
                    .Single(t => t.Status.Equals("failed", System.StringComparison.OrdinalIgnoreCase))
                    .TestOutput;

            _testCasesReported.ShouldSatisfyAllConditions(
                () => _testCasesReported.Count.ShouldBe(7),
                () => _testCasesReported
                    .Count(t => t.Status.Equals("failed", System.StringComparison.OrdinalIgnoreCase))
                    .ShouldBe(1),
                 () => faileDetstOutput.ShouldContain("error creating cluster"),
                 () => faileDetstOutput.ShouldContain("\nSecond string")
                );
        }

        [Test]
        public void Test_ReadFileWithFailureAndSkip()
        {
            // Arrange 
            var xmlReader = TestData.GetFile("FailedAndSkipped.xml");

            // Act 
            Task.WaitAll(_reader.Read(xmlReader, "tr1", "develop", ""));

            // Assert
            var faileDetstOutput = _testCasesReported
                    .Single(t => t.Status.Equals("failed", System.StringComparison.OrdinalIgnoreCase))
                    .TestOutput;

            _testCasesReported.ShouldSatisfyAllConditions(
                () => _testCasesReported.Count.ShouldBe(3),
                () => _testCasesReported
                    .Count(t => t.Status.Equals("failed", System.StringComparison.OrdinalIgnoreCase))
                    .ShouldBe(1),
                () => _testCasesReported
                    .Count(t => t.Status.Equals("passed", System.StringComparison.OrdinalIgnoreCase))
                    .ShouldBe(2)
                );
        }
    }
}