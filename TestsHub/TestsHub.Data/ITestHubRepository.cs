using System;
using System.Collections.Generic;
using System.Text;
using TestsHub.Data.DataModel;

namespace TestsHub.Data
{
    public interface ITestHubRepository
    {
        string Organisation { get; }
        TestRun GetTestRun(string project, string testRunId);
        dynamic GetProjectSummary(string project);
    }
}
