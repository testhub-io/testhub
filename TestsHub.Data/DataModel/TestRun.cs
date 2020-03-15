using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestsHub.Data.DataModel
{
    public class TestRun
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TestRunName { get; set; }

        public virtual ICollection<TestCase> TestCases { get; set; }        

        public virtual Project Project { get; set; }

        public int ProjectId { get; set; }

        public DateTime Timestamp { get; set; }

        public decimal Time { get; set; }
        
        public string Name { get; set; }

        public TestResult Status { get; set; }

        public virtual Coverage Coverage { get; set; }

        public int TestCasesCount { get; set; }

        public string Branch { get; set; }

        public string CommitId { get; set; }
    }
}
