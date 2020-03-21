using TestHub.Data.DataModel;

namespace TestHub.Data
{
    public interface ITestHubRepository
    {
        string Organisation { get; }
        Api.Data.TestRun GetTestRun(string project, string testRunId);
        Api.Data.Project GetProjectSummary(string project);
        Api.Data.Organisation GetOrgSummary(string org);

        TestHubDBContext TestHubDBContext { get; }
    }
}
