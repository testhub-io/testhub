using System;
using System.Collections.Generic;
using System.Text;

namespace TestHub.Data.DataModel
{
    public class TestRunExtended : TestRun
    {
        public string ProjectName { get; set; }
        
        public decimal CoveragePercent { get; set; }
     
    }
}
