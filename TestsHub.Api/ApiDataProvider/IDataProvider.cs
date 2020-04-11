using System.Collections.Generic;
using TestHub.Data.DataModel;

namespace TestHub.Api.ApiDataProvider
{
    public interface IDataProvider
    {
        string Organisation { get; }
        
        Data.TestRun GetTestRun(string project, string testRunId);
                        
        Data.Organisation GetOrgSummary();

        TestHubDBContext TestHubDBContext { get; }

        IEnumerable<Data.ProjectSummary> GetProjects();

        Data.ProjectSummary GetProjectSummary(string project);
    }
}
