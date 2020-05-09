using System;
using System.Collections.Generic;
using System.Text;

namespace TestHub.Api.Data
{
    public class CoverageHistoricalData
    {
        public IEnumerable<CoverageDataItem> Items { get; set; }
    }

    public class CoverageDataItem
    {
        public DateTime DateTime { get; set; }
        public decimal Coverage { get; set; }
    }
}
