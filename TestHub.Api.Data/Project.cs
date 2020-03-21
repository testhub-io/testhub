using System;
using System.Collections.Generic;
using System.Text;

namespace TestHub.Api.Data
{
    public class Project
    {
        public string Name { get; set; }
        public int TestRunsCount { get; set; }
        public List<TestRunSummary> TestRuns { get; set; }
    }
}
