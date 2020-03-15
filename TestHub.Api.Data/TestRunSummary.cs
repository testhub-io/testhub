namespace TestsHub.Api.Data
{
    public class TestRunSummary
    {
        public int TestsCount { get; set; }
        public int Passed { get; set; }
        public int Failed { get; set; }
        public int Skipped { get; set; }
    }
}