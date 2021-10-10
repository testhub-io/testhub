namespace TestHub.Data.DataModel
{
    public class TestRunExtended : TestRun
    {
        public string ProjectName { get; set; }

        public decimal CoveragePercent { get; set; }

        public decimal CoverageGrowth { get; set; }

        public decimal TestsCountGrowth { get; set; }

    }
}
