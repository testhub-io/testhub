using System.Collections.Generic;

namespace TestHub.Api.Data
{
    public class TestGridData : DataObjectBase
    {
        public List<TestsCategory> Tests { get; set; }
        public IEnumerable<TestRunTestData>  Data { get; set; }
    }

    public class TestsCategory
    {
        public string ClassName { get; set; }
        public List<TestCaseNameIdPair> Test { get; set; }
    }

    public class TestCaseNameIdPair
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    public class TestRunTestData : DataObjectBase
    {        
        public string TestRun { get; set; }

        public IEnumerable<TestCaseWithResult> TestCases { get; set; }

    }

    public class TestCaseWithResult
    {        
        public short Status { get; set; }
        public int Id { get; set; }
    }
}
