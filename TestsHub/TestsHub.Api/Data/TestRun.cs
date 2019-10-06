using System.Collections.Generic;

namespace TestsHub.Api.Data
{
    public class TestRun
    {
        public string Name { get; set;}
        
        public ICollection<TestCase> TestCases { get; set; }
    }
}
