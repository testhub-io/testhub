using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestsHub.Api.Data
{
    public class Organisation : DataObjectBase
    {
        public string Name { get; set; }


        public List<ProjectSummary> Projects { get; set; } = new List<ProjectSummary>();
}
}
