
using TestsHubUploadEndpoint.ReportModel;

namespace TestsHubUploadEndpoint
{
    public interface IVisitor
    {
        void TestCaseAdded(TestCase testCase);
        void SetCurrentTestRun(TestRun testRun);
    }
}