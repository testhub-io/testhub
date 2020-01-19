using TestsHub.Data.DataModel;

namespace TestsHubUploadEndpoint
{
    internal interface IVisitor
    {
        void TestCaseAdded(TestCase testCase);
        void SetCurrentTestRun(TestRun testRun);
    }
}