namespace TestHub.Data.DataModel
{
    public class ProjectExtended
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int TestRunsCount { get; set; }

        public decimal Coverage { get; set; }

        public int TestCasesCount { get; set; }
    }
}
