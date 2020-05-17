using System;
using System.Collections.Generic;
using System.Text;

namespace TestHub.Api.Data
{
    public class TestRunTests : DataObjectBase
    {
        public IEnumerable<TestCase> Tests { get; set; }
        
    }
}
