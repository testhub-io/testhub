using System.ComponentModel.DataAnnotations;

namespace TestHub.Data.DataModel
{
    public class Coverage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TestRunId { get; set; }

        public virtual TestRun TestRun { get; set; }

        public int LinesCovered { get; set; }

        public int LinesValid { get; set; }

        public decimal Percent
        {
            get
            {
                return (LinesCovered / LinesValid) * 100;
            }
        }
    }
}
