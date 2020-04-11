using System;
using System.Collections.Generic;
using System.Text;

namespace TestHub.Api.Data
{
    public class CoverageHistoricalData
    {
        public IEnumerable<DataItem> Items { get; set; }
    }

    public class DataItem
    {
        public DateTime DateTime { get; set; }
        public decimal Coverage { get; set; }
    }
}
