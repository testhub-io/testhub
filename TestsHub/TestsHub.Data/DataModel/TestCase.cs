using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestsHub.Data.DataModel
{
    public class TestCase
    {
        [Key]
        public long Id { get; set; }
        public int TestRunId { get; set; }

        public string Name { get; set; }

        public string ClassName { get; set; }

        public string SystemOut { get; set; }

        public string Status { get; set; }
        public string Time { get; set; }
        public string File { get; set; }
    }
}
