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
            var xmlReader = TestData.GetTestReport("junit", "MinimalJUnit.xml");

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
            _testCasesReported.Select(t => t.TestSuite.Name).Distinct().Count().ShouldBe(1);
        }

        [Test]
        public void Test_ReadFileWithFailure()
        {
            // Arrange 
            var xmlReader = TestData.GetTestReport("junit", "failure.xml");

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
            var xmlReader = TestData.GetTestReport("junit", "FailedAndSkipped.xml");

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

        [Test]
        public void Test_ReadFileWithCoupleOfTestSuites()
        {
            // Arrange
            var xmlReader = TestData.GetTestReport("junit", "redundant-suite-issue.xml");

            // Act
            Task.WaitAll(_reader.Read(xmlReader, "tr1", "develop", ""));

            // Assert
            var d = _testCasesReported.Select(t => t.TestSuite.Name).Distinct();
            foreach (var v  in d)
            {
                System.Diagnostics.Debug.WriteLine(v);
            }

            _testCasesReported.ShouldSatisfyAllConditions(
                () => _testCasesReported.Count.ShouldBe(158),
                () => d.Count().ShouldBe(18));
        }

        [Test]
        public void Test_VisitorIsCalledForEachTestCase()
        {
            // Arrange
            var visitorMock = new Mock<IVisitor>();
            var visitedTestCases = new List<TestCase>();
            visitorMock.Setup(v => v.TestCaseAdded(It.IsAny<TestCase>()))
                .Callback<TestCase>(tc => visitedTestCases.Add(tc));

            _reader.Visitor = visitorMock.Object;
            var xmlReader = TestData.GetTestReport("junit", "MinimalJUnit.xml");

            // Act
            Task.WaitAll(_reader.Read(xmlReader, "tr1", "develop", "commit123"));

            // Assert
            visitorMock.Verify(v => v.TestCaseAdded(It.IsAny<TestCase>()), Times.Exactly(2));
            visitedTestCases.Count.ShouldBe(2);
            visitedTestCases[0].Name.ShouldBe("PassedTest");
            visitedTestCases[1].Name.ShouldBe("TestMethod1");
        }

        [Test]
        public void Test_TestRunStatusIsFailedWhenAnyTestFails()
        {
            // Arrange
            var xmlReader = TestData.GetTestReport("junit", "failure.xml");

            // Act
            Task.WaitAll(_reader.Read(xmlReader, "tr1", "develop", "commit123"));

            // Assert
            _testRunReported.Status.ShouldBe("failed");
        }

        [Test]
        public void Test_TestRunStatusIsPassedWhenAllTestsPass()
        {
            // Arrange
            var xmlReader = TestData.GetTestReport("junit", "MinimalJUnit.xml");

            // Act
            Task.WaitAll(_reader.Read(xmlReader, "tr1", "develop", "commit123"));

            // Assert
            _testRunReported.Status.ShouldBe("passed");
        }

        [Test]
        public void Test_TestRunPropertiesAreSetCorrectly()
        {
            // Arrange
            var xmlReader = TestData.GetTestReport("junit", "MinimalJUnit.xml");
            var testRunName = "MyTestRun";
            var branch = "feature/test-branch";
            var commitId = "abc123def456";

            // Act
            Task.WaitAll(_reader.Read(xmlReader, testRunName, branch, commitId));

            // Assert
            _testRunReported.ShouldSatisfyAllConditions(
                () => _testRunReported.TestRunName.ShouldBe(testRunName),
                () => _testRunReported.Branch.ShouldBe(branch),
                () => _testRunReported.CommitId.ShouldBe(commitId),
                () => _testRunReported.TestCasesCount.ShouldBe(2),
                () => _testRunReported.Timestamp.ShouldBeInRange(
                    System.DateTime.UtcNow.AddMinutes(-1),
                    System.DateTime.UtcNow.AddMinutes(1))
            );
        }

        [Test]
        public void Test_TestCaseWithErrorStatus()
        {
            // Arrange
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<testsuites>
    <testsuite name=""TestSuite"">
        <testcase name=""ErrorTest"" classname=""Test.Class"" time=""0.5"">
            <error message=""Error occurred"">Stack trace here</error>
        </testcase>
    </testsuite>
</testsuites>";
            var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));

            // Act
            Task.WaitAll(_reader.Read(stream, "tr1", "develop", ""));

            // Assert
            var testCase = _testCasesReported.Single();
            testCase.ShouldSatisfyAllConditions(
                () => testCase.Status.ShouldBe("failed"),
                () => testCase.TestOutput.ShouldContain("Stack trace here")
            );
        }

        [Test]
        public void Test_TestCaseWithSystemErrStatus()
        {
            // Arrange
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<testsuites>
    <testsuite name=""TestSuite"">
        <testcase name=""SystemErrTest"" classname=""Test.Class"" time=""0.5"">
            <system-err>System error output</system-err>
        </testcase>
    </testsuite>
</testsuites>";
            var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));

            // Act
            Task.WaitAll(_reader.Read(stream, "tr1", "develop", ""));

            // Assert
            var testCase = _testCasesReported.Single();
            testCase.ShouldSatisfyAllConditions(
                () => testCase.Status.ShouldBe("failed"),
                () => testCase.TestOutput.ShouldContain("System error output")
            );
        }

        [Test]
        public void Test_TestCaseWithSkippedStatus()
        {
            // Arrange
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<testsuites>
    <testsuite name=""TestSuite"">
        <testcase name=""SkippedTest"" classname=""Test.Class"" time=""0"">
            <skipped message=""Test skipped""/>
        </testcase>
    </testsuite>
</testsuites>";
            var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));

            // Act
            Task.WaitAll(_reader.Read(stream, "tr1", "develop", ""));

            // Assert
            var testCase = _testCasesReported.Single();
            testCase.Status.ShouldBe("skipped");
        }

        [Test]
        public void Test_TestCaseWithSystemOut()
        {
            // Arrange
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<testsuites>
    <testsuite name=""TestSuite"">
        <testcase name=""PassedTest"" classname=""Test.Class"" time=""0.1"">
            <system-out>Standard output</system-out>
        </testcase>
    </testsuite>
</testsuites>";
            var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));

            // Act
            Task.WaitAll(_reader.Read(stream, "tr1", "develop", ""));

            // Assert
            var testCase = _testCasesReported.Single();
            testCase.ShouldSatisfyAllConditions(
                () => testCase.Status.ShouldBe("passed"),
                () => testCase.TestOutput.ShouldContain("Standard output")
            );
        }

        [Test]
        public void Test_TestCaseWithoutClassName()
        {
            // Arrange
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<testsuites>
    <testsuite name=""TestSuite"">
        <testcase name=""TestWithoutClassName"" time=""0.1""/>
    </testsuite>
</testsuites>";
            var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));

            // Act
            Task.WaitAll(_reader.Read(stream, "tr1", "develop", ""));

            // Assert
            var testCase = _testCasesReported.Single();
            testCase.ClassName.ShouldBeNull();
        }

        [Test]
        public void Test_TestCaseWithInvalidTimeDefaultsToZero()
        {
            // Arrange
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<testsuites>
    <testsuite name=""TestSuite"">
        <testcase name=""TestWithInvalidTime"" classname=""Test.Class"" time=""invalid""/>
    </testsuite>
</testsuites>";
            var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));

            // Act
            Task.WaitAll(_reader.Read(stream, "tr1", "develop", ""));

            // Assert
            _testCasesReported.Single().Time.ShouldBe(0);
        }

        [Test]
        public void Test_TestCaseWithMissingTimeDefaultsToZero()
        {
            // Arrange
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<testsuites>
    <testsuite name=""TestSuite"">
        <testcase name=""TestWithoutTime"" classname=""Test.Class""/>
    </testsuite>
</testsuites>";
            var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));

            // Act
            Task.WaitAll(_reader.Read(stream, "tr1", "develop", ""));

            // Assert
            _testCasesReported.Single().Time.ShouldBe(0);
        }

        [Test]
        public void Test_TestSuiteAttributesAreParsedCorrectly()
        {
            // Arrange
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<testsuites>
    <testsuite name=""MySuite"" hostname=""localhost"" package=""com.test""
               junit_id=""suite1"" timestamp=""2024-01-15T10:30:00"" time=""1.5"">
        <testcase name=""Test1"" classname=""Test.Class"" time=""0.5""/>
    </testsuite>
</testsuites>";
            var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));

            // Act
            Task.WaitAll(_reader.Read(stream, "tr1", "develop", ""));

            // Assert
            var testSuite = _testCasesReported.Single().TestSuite;
            testSuite.ShouldSatisfyAllConditions(
                () => testSuite.Name.ShouldBe("MySuite"),
                () => testSuite.Hostname.ShouldBe("localhost"),
                () => testSuite.Package.ShouldBe("com.test"),
                () => testSuite.JUnitId.ShouldBe("suite1"),
                () => testSuite.Timestamp.ShouldBe(System.DateTime.Parse("2024-01-15T10:30:00")),
                () => testSuite.Time.ShouldBe(1.5m)
            );
        }

        [Test]
        public void Test_TestCaseWithMultipleOutputElements()
        {
            // Arrange
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<testsuites>
    <testsuite name=""TestSuite"">
        <testcase name=""TestMultiOutput"" classname=""Test.Class"" time=""0.5"">
            <failure message=""First failure"">First output</failure>
            <system-out>System output</system-out>
            <system-err>Error output</system-err>
        </testcase>
    </testsuite>
</testsuites>";
            var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));

            // Act
            Task.WaitAll(_reader.Read(stream, "tr1", "develop", ""));

            // Assert
            var testCase = _testCasesReported.Single();
            testCase.TestOutput.ShouldSatisfyAllConditions(
                () => testCase.TestOutput.ShouldContain("First output"),
                () => testCase.TestOutput.ShouldContain("System output"),
                () => testCase.TestOutput.ShouldContain("Error output")
            );
        }

        [Test]
        public void Test_EmptyTestSuites()
        {
            // Arrange
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<testsuites>
    <testsuite name=""EmptySuite""/>
</testsuites>";
            var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));

            // Act
            Task.WaitAll(_reader.Read(stream, "tr1", "develop", ""));

            // Assert
            _testCasesReported.Count.ShouldBe(0);
            _testRunReported.TestCasesCount.ShouldBe(0);
            _testRunReported.Status.ShouldBe("passed");
        }

        [Test]
        public void Test_TimeWithScientificNotation()
        {
            // Arrange
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<testsuites>
    <testsuite name=""TestSuite"" time=""0.0015"">
        <testcase name=""Test1"" classname=""Test.Class"" time=""1.23E-4""/>
    </testsuite>
</testsuites>";
            var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));

            // Act
            Task.WaitAll(_reader.Read(stream, "tr1", "develop", ""));

            // Assert
            var testCase = _testCasesReported.Single();
            testCase.Time.ShouldBe(0.000123m);
            testCase.TestSuite.Time.ShouldBe(0.0015m);
        }

        [Test]
        public void Test_MultipleTestSuitesWithDifferentNames()
        {
            // Arrange
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<testsuites>
    <testsuite name=""Suite1"">
        <testcase name=""Test1"" classname=""Test.Class1"" time=""0.1""/>
    </testsuite>
    <testsuite name=""Suite2"">
        <testcase name=""Test2"" classname=""Test.Class2"" time=""0.2""/>
    </testsuite>
</testsuites>";
            var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));

            // Act
            Task.WaitAll(_reader.Read(stream, "tr1", "develop", ""));

            // Assert
            _testCasesReported.Count.ShouldBe(2);
            _testCasesReported[0].TestSuite.Name.ShouldBe("Suite1");
            _testCasesReported[1].TestSuite.Name.ShouldBe("Suite2");
        }

        [Test]
        public void Test_TestCaseWithNoChildElementsHasNoTestOutput()
        {
            // Arrange
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<testsuites>
    <testsuite name=""TestSuite"">
        <testcase name=""SimpleTest"" classname=""Test.Class"" time=""0.1""/>
    </testsuite>
</testsuites>";
            var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));

            // Act
            Task.WaitAll(_reader.Read(stream, "tr1", "develop", ""));

            // Assert
            var testCase = _testCasesReported.Single();
            testCase.TestOutput.ShouldBeNull();
        }

        [Test]
        public void Test_CaseInsensitiveTestSuiteAndTestCaseElements()
        {
            // Arrange - TestSuite and TestCase (capitalized) should be recognized
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<testsuites>
    <TestSuite name=""TestSuite"">
        <TestCase name=""Test1"" classname=""Test.Class"" time=""0.1"">
            <failure message=""Test failed"">Error details</failure>
        </TestCase>
    </TestSuite>
</testsuites>";
            var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));

            // Act
            Task.WaitAll(_reader.Read(stream, "tr1", "develop", ""));

            // Assert
            _testCasesReported.Count.ShouldBe(1);
            var testCase = _testCasesReported.Single();
            testCase.Name.ShouldBe("Test1");
            testCase.Status.ShouldBe("failed");
        }
    }
}