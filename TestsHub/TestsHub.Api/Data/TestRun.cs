using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestsHub.Api.Data
{
    public class TestRun
    {
        public string Id { get; set;}
        public List<TestCase> TestCases { get; set; }
    }
}
