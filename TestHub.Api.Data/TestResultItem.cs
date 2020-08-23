using System;

namespace TestHub.Api.Data
{
    public class TestResultItem : DataObjectBase
    {
        public TestResult Status { get; set; }

        public string TestRunName { get; set; }

        public DateTime Timestamp { get; set; }       
    }
}
