using System;
using System.Collections.Generic;
using System.Text;

namespace TestHub.Api.Data
{
    public class TestResultsHistoricalData : DataObjectBase
    {
        public IEnumerable<TestResultsDataItem> Data { get; set; }
    }

    public class TestResultsDataItem
    {
        public DateTime DateTime { get; set; }
        
        public int Passed { get; set; }

        public int Failed { get; set; }

        public int Skipped { get; set; }

        public string Name { get; set; }
    }
}
