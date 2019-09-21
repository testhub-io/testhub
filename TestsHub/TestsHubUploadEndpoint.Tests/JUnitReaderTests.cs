using Moq;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using TestsHubUploadEndpoint;
using TestsHubUploadEndpoint.DataModel;
using TestsHubUploadEndpoint.Tests.TestData;

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
            var caseseReported = new List<TestCase>();
            dataLoaderMock.Setup(s => s.Add(It.IsAny<TestCase>())).Callback<TestCase>(t => caseseReported.Add(t));

            var reader = new JUnitReader(dataLoaderMock.Object);

            var xmlReader = TestData.GetFile("MInimalJUnit.xml");

            // Act 
            Task.WaitAll(reader.Read(xmlReader));

            // Assert
            caseseReported.Count.ShouldBe(2);

            var case1 = caseseReported[0];
            case1.Name.ShouldBe("PassedTest");
            case1.ClassName .ShouldBe("aspnetappDependency.Tests.UnitTest1");
            case1.Status.ShouldBeNull();
            case1.Time.ShouldBe("0.0000749");

        }
    }
}