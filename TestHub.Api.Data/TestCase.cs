using System.Collections.Generic;

namespace TestHub.Api.Data
{
    public class TestCase
    {
        public string Name { get; set; }

        public string ClassName { get; set; }

        public string SystemOut { get; set; }

        public TestResult Status { get; set; }

        public decimal Time { get; set; }

        public string File { get; set; }
 
        public IEnumerable<TestResult> RecentResults { get; set; }
    }
}