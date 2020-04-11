using System;

namespace TestsHubUploadEndpoint.ReportModel
{
    public class TestSuite
    {
        public string Name { get; set; }

        public string Hostname { get; set; }
        public string Package { get; set; }
        public string JUnitId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Time { get; set; }

    }
}
