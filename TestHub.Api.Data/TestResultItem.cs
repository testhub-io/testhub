using System;
using System.Collections.Generic;
using System.Text;

namespace TestHub.Api.Data
{
    public class TestResultItem : DataObjectBase
    {
        public TestResult Status { get; set; }
        public string TestRunName { get; set; }

        public DateTime Timestemp { get; set; }       
    }
}
