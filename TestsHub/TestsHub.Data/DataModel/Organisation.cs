using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestsHub.Data.DataModel
{
    public class Organisation
    {
        [Key]
        public string Name { get; set; }

        public List<Project> Projects { get; set; }

    }
}
