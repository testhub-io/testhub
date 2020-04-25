using System.Collections.Generic;
using TestHub.Api.Data;
using TestHub.Data.DataModel;

namespace TestHub.Api.ApiDataProvider
{
    public interface IDataProvider
    {
        string Organisation { get; }
        
        Data.TestRun GetTestRun(string project, string testRunId);
                        
        Data.Organisation GetOrgSummary();

        TestHubDBContext TestHubDBContext { get; }

        IEnumerable<ProjectSummary> GetProjects();

        ProjectSummary GetProjectSummary(string project);

        IEnumerable<TestRunSummary> GetTestRuns(string projectName);
    }
}
