using System;
using System.Collections.Generic;
using TestsHubUploadEndpoint.ReportModel;

namespace TestsHubUploadEndpoint
{
    public interface IDataLoader : IDisposable
    {
        void Add(TestRun testRun, IEnumerable<TestCase> testCases);       
    }
}