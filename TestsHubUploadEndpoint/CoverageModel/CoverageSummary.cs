using System;
using System.Collections.Generic;
using System.Text;

namespace TestsHubUploadEndpoint.CoverageModel
{
    public class CoverageSummary
    {
        public CoverageSummary(int linesCovered, int linesValid, string testRunName)
        {
            LinesCovered = linesCovered;
            LinesValid = linesValid;
            TestRunName = testRunName;
        }

        public int LinesCovered { get; private set; }
        
        public int LinesValid { get; private set; }

        public string TestRunName { get; set; }
    }
}
