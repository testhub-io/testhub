namespace TestHub.Api.Data
{
    public class OrgSummary
    {
        public object ProjectsCount { get; set; }
        public double AvgTestsCount { get; set; }
        public decimal AvgCoverage { get; set; }

        public int ProjectsInGreen { get; set; }
        public decimal ProjectsInRed { get; set; }
    }
}
