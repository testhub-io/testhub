using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestsHub.Data.DataModel
{
    public class Project
    {       
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<TestRun> TestRuns { get; set; } = new List<TestRun>();
    }
}
