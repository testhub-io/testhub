using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestsHub.Data.DataModel
{
    public class TestRun
    {
        [Key]
        public int Id { get; set; }
        public string TestRunName { get; set; }

        public IEnumerable<TestCase> TestCases { get; set; }
    }
}
