using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestsHub.Data.DataModel
{
    public class Organisation
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    }
}
