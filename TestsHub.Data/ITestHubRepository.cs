using TestsHub.Data.DataModel;

namespace TestsHub.Data
{
    public interface ITestHubRepository
    {
        string Organisation { get; }
        dynamic GetTestRun(string project, string testRunId);
        dynamic GetProjectSummary(string project);
        dynamic GetOrgSummary(string org);

        TestHubDBContext TestHubDBContext { get; }
    }
}
