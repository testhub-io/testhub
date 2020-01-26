
using TestsHubUploadEndpoint.ReportModel;

namespace TestsHubUploadEndpoint
{
    internal interface IVisitor
    {
        void TestCaseAdded(TestCase testCase); 
        void SetCurrentTestRun(TestRun testRun);  
    }
}