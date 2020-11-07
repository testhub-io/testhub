using System.Collections.Generic;

namespace TestHub.Api.Data
{
    public class TestGridData : DataObjectBase
    {
        public List<string> TestCaseNames { get; set; }
        public IEnumerable<TestRunTestData>  Data { get; set; }
    }

    public class TestRunTestData : DataObjectBase
    {        
        public string TestRunName { get; set; }

        public IEnumerable<TestCaseWithResult> TestCases { get; set; }

    }

    public class TestCaseWithResult
    {
        public string Name { get; set; }
        public short Status { get; set; }
    }
}
