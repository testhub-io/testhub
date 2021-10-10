using System;
using System.Collections.Generic;
using System.Text;

namespace TestHub.Api.Data
{
    public class CoverageHistoricalData : DataObjectBase
    {
        public IEnumerable<CoverageDataItem> Items { get; set; }
    }

    public class CoverageDataItem
    {
        public DateTime DateTime { get; set; }
        public decimal Coverage { get; set; }

        public string TestRunName { get; set; }

        public string Uri { get; set; }
    }
}
