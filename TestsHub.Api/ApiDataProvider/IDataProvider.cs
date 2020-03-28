using System.Collections.Generic;
using TestHub.Data.DataModel;

namespace TestHub.Api.ApiDataProvider
{
    public interface IDataProvider
    {
        string Organisation { get; }
        Data.TestRun GetTestRun(string project, string testRunId);
        Data.Project GetProjectSummary(string project);
        Data.Organisation GetOrgSummary(string org);

        TestHubDBContext TestHubDBContext { get; }

        IEnumerable<Data.ProjectSummary> GetProjects(string org);
    }
}
