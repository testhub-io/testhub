using System.Collections.Generic;
using System.Linq;
using TestHub.Api.Data;
using TestHub.Data.DataModel;

namespace TestHub.Api.ApiDataProvider
{
    public interface IDataProvider
    {
        string Organisation { get; }
        
        Data.TestRun GetTestRunSummary(string project, string testRunId);
                        
        Data.Organisation GetOrgSummary();

        TestHubDBContext TestHubDBContext { get; }

        IQueryable<ProjectSummary> GetProjects();

        ProjectSummary GetProjectSummary(string project);

        IEnumerable<TestRunSummary> GetTestRuns(string projectName);
        
        TestResultsHistoricalData GetTestResultsForProject(string project);

        TestRunTests GetTests(string projectName, string testRunName);
        Data.CoverageHistoricalData GetCoverageHistory(string project);
    }
}
