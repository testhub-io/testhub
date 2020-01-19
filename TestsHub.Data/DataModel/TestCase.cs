using System.ComponentModel.DataAnnotations;

namespace TestsHub.Data.DataModel
{
    public class TestCase
    {
        [Key]
        public long Id { get; set; }

        public int TestRunId { get; set; }

        [Required]
        public string Name { get; set; }

        public string ClassName { get; set; }

        public string SystemOut { get; set; }

        [Required]
        public string Status { get; set; } = "passed";

        public decimal Time { get; set; }

        public string File { get; set; }

        public virtual TestRun TestRun { get; set; }

        public virtual TestSuite TestSuite { get; set; }

        public int? TestSuiteId { get; set; }

        // there could be several of them, consider separate records
        public string TestOutput { get; set; }
    }
}
