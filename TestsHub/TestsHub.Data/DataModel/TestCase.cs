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
        public string Status { get; set; }

        public string Time { get; set; }

        public string File { get; set; }

        public virtual TestRun TestRun { get; set; }
    }
}
