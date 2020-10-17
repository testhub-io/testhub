using System;
using System.Collections.Generic;
using System.Text;

namespace TestHub.Api.Data
{
    public class TestCountHistoricalData : DataObjectBase
    {
        public IEnumerable<TestCountDataItem> Data { get; set; }
    }

    public class TestCountDataItem : DataObjectBase
    {
        public DateTime DateTime { get; set; }
        
        public int TestsCount { get; set; }
    }
}
