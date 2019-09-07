using System;
using TestsHubUploadEndpoint.DataModel;

namespace TestsHubUploadEndpoint
{
    public interface IDataLoader : IDisposable
    {
        void Add(TestRun testRun);
        void Add(TestCase testCase);
    }
}