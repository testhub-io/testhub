using System;
using System.Collections.Generic;
using TestsHubUploadEndpoint.Coverage;
using TestsHubUploadEndpoint.ReportModel;

namespace TestsHubUploadEndpoint
{
    public interface IDataLoader : IDisposable
    {
        void Add(TestRun testRun, IEnumerable<TestCase> testCases);
        void Add(CoverageSummary coverageSummary);
    }
}