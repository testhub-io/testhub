using System;
using System.Collections.Generic;
using System.Text;

namespace TestHub.Data.DataModel
{
    public class TestResultsHistoricalItem
    {
        public int Id { get; set; }

        public string TestRunName { get; set; }

        public DateTime Timestamp { get; set; }

        public int Status { get; set; }

        public int Count { get; set; }        
    }
}
