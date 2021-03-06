using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TestHub.Api.Data
{
    public class TestRunSummary : DataObjectBase
    {
        public string Name { get; set; }

        public DateTime Timestamp { get; set; }

        public decimal Time { get; set; }

        public string Branch { get; set; }

        public TestResult Result { get; set; }
                
        public decimal? Coverage { get; set; }

        public decimal TestCountGrowth { get; set; }

        public decimal? CoverageGrowth { get; set; }

        public TestRunStats Stats { get; set; }
    }
}