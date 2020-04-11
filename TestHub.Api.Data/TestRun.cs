using System;
using System.Collections.Generic;

namespace TestHub.Api.Data
{
    public class TestRun : DataObjectBase
    {
        public string Name { get; set; }

        public string Branch { get; set; }

        public string CommitId { get; set; }

        public TestResult Status { get; set; }

        public DateTime Timestamp { get; set; }

        public int TestCasesCount { get; set; }

        public decimal? Coverage { get; set; }

        public TestRunSummary Summary { get; set; }

        public decimal Time { get; set; }

        public IEnumerable<TestCase> TestCases { get; set; }
    }
}
