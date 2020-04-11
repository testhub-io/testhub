using System;

namespace TestHub.Api.Data
{
    public class TestRunSummary : DataObjectBase
    {
        public string Name;

        public int TestsCount { get; set; }
        public int Passed { get; set; }
        public int Failed { get; set; }
        public int Skipped { get; set; }
        public DateTime TimeStemp { get; set; }
        public decimal Time { get; set; }
        public TestRunStats Count { get; set; }
    }
}