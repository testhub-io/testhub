using System;
using System.Collections.Generic;
using System.Text;

namespace TestsHubUploadEndpoint.ReportModel
{
    public class TestRun
    {
        public string TestRunName { get; set; }

        public int ProjectId { get; set; }

        public DateTime Timestamp { get; set; }

        public decimal Time { get; set; }
        
        public string Name { get; set; }

        public string Status { get; set; }
        public int TestCasesCount { get; internal set; }
    }
}
