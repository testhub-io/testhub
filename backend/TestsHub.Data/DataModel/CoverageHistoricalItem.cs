using System;
using System.Collections.Generic;
using System.Text;

namespace TestHub.Data.DataModel
{
    public class CoverageHistoricalItem
    {
        public string TestRunName { get; set; }

        public DateTime Timestamp { get; set; }
        
        public decimal Percent { get; set; }
    }
}
