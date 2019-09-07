using System;
using System.Collections.Generic;
using System.Text;

namespace TestsHubUploadEndpoint.DataModel
{
    public class TestCase
    {
        public int TestRunId { get; set; }

        public string Name { get; set; }

        public string ClassName { get; set; }

        public string SystemOut { get; set; }

        public string Status { get; set; }
        public string Time { get; internal set; }
    }
}
