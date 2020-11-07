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

        TestCountHistoricalData GetTestResultsForOrganisation();

        TestRunTests GetTests(string projectName, string testRunName);

        CoverageHistoricalData GetCoverageHistory(string project);

        TestGridData GetTestGrid(string project);
    }
}
