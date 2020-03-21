using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestHub.Data.DataModel
{
    public class TestSuite
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        public virtual ICollection<TestCase> TestCases { get; set; }

        public string Hostname { get; set; }
        public string Package { get; set; }
        public string JUnitId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Time { get; set; }
        
    }
}
