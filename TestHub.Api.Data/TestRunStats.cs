using System;
using System.Collections.Generic;
using System.Text;

namespace TestHub.Api.Data
{
    public class TestRunStats
    {
        public int Passed { get; set; }
        public int Failed { get; set; }
        public int Skipped { get; set; }
    }
}
