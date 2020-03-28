using System;
using System.Collections.Generic;
using System.Text;

namespace TestHub.Api.Data
{
    public class OrgSummary
    {
        public object ProjectsCount { get; set; }
        public double AvgTestsCount { get; set; }
        public decimal AvgCoverage { get; set; }
    }
}
