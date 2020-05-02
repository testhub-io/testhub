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


        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_Read()
        {
            // Arrange 
            var dataLoaderMock = new Mock<IDataLoader>();
            var testRunReported = new TestRun();
            var testCasesReported = new List<TestCase>();
            dataLoaderMock.Setup(s => s.Add(It.IsAny<TestRun>(), It.IsAny<IEnumerable<TestCase>>()))
                .Callback<TestRun, IEnumerable<TestCase>>((t, c) =>
                {
                    testRunReported = t;
                    testCasesReported = c.ToList();
                });


            var reader = new JUnitReader(dataLoaderMock.Object);

            var xmlReader = TestData.GetFile("MinimalJUnit.xml");

            // Act 
            Task.WaitAll(reader.Read(xmlReader, "tr1", "develop", ""));

            // Assert
            testCasesReported.Count.ShouldBe(2);

            var case1 = testCasesReported.First();
            case1.Name.ShouldBe("PassedTest");
            case1.ClassName.ShouldBe("aspnetappDependency.Tests.UnitTest1");
            case1.Status.ShouldBe("passed");
            case1.Time.ShouldBe(0.0000749m);
            case1.TestSuite.Name.ShouldBe("aspnetappDependency.Tests.UnitTest1");
        }

        [Test]
        public void Test_ReadFileWithFailure()
        {
            // Arrange 
            var dataLoaderMock = new Mock<IDataLoader>();
            var testRunReported = new TestRun();
            var testCasesReported = new List<TestCase>();
            dataLoaderMock.Setup(s => s.Add(It.IsAny<TestRun>(), It.IsAny<IEnumerable<TestCase>>()))
                .Callback<TestRun, IEnumerable<TestCase>>((t, c) =>
                {
                    testRunReported = t;
                    testCasesReported = c.ToList();
                });


            var reader = new JUnitReader(dataLoaderMock.Object);

            var xmlReader = TestData.GetFile("failure.xml");

            // Act 
            Task.WaitAll(reader.Read(xmlReader, "tr1", "develop", ""));

            // Assert
            var faileDetstOutput = testCasesReported
                    .Single(t => t.Status.Equals("failed", System.StringComparison.OrdinalIgnoreCase))
                    .TestOutput;

            testCasesReported.ShouldSatisfyAllConditions(
                () => testCasesReported.Count.ShouldBe(7),
                () => testCasesReported
                    .Count(t => t.Status.Equals("failed", System.StringComparison.OrdinalIgnoreCase))
                    .ShouldBe(1),
                 () => faileDetstOutput.ShouldContain("error creating cluster"),
                 () => faileDetstOutput.ShouldContain("\nSecond string")
                );


        }
    }
}