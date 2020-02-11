using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestsHub.Data.DataModel
{
    public class Coverage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TestRunId { get; set; }

        public virtual TestRun TestRun { get; set; }

        public int LinesCovered { get; private set; }

        public int LinesValid { get; private set; }
    }
}
