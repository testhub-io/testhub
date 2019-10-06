using Moq;
using NUnit.Framework;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using TestsHubUploadEndpoint;
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
        public void Test_Read ()
        {
            // Arrange 
            var dataLoaderMock = new Mock<IDataLoader>();
            var testRunReported = new TestsHub.Data.DataModel.TestRun();
            dataLoaderMock.Setup(s => s.Add(It.IsAny<TestsHub.Data.DataModel.TestRun>()))
                .Callback<TestsHub.Data.DataModel.TestRun>(t => testRunReported = t);

            var reader = new JUnitReader(dataLoaderMock.Object);

            var xmlReader = TestData.GetFile("MInimalJUnit.xml");

            // Act 
            Task.WaitAll(reader.Read(xmlReader, "tr1"));

            // Assert
            testRunReported.TestCases.Count.ShouldBe(2);

            var case1 = testRunReported.TestCases.First();
            case1.Name.ShouldBe("PassedTest");
            case1.ClassName .ShouldBe("aspnetappDependency.Tests.UnitTest1");
            case1.Status.ShouldBeNull();
            case1.Time.ShouldBe("0.0000749");
        }     
    }
}