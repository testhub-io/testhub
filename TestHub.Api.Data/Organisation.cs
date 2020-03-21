using System.Collections.Generic;

namespace TestHub.Api.Data
{
    public class Organisation : DataObjectBase
    {
        public string Name { get; set; }


        public List<ProjectSummary> Projects { get; set; } = new List<ProjectSummary>();
    }
}
