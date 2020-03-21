using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestHub.Data.DataModel
{
    public class Project
    {       
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int OrganisationId { get; set; }

        public virtual ICollection<TestRun> TestRuns { get; set; }

        public virtual Organisation Organisation { get; set; }
    }
}
