using System;
using TestsHub.Data.DataModel;

namespace TestsHubUploadEndpoint
{
    public interface IDataLoader : IDisposable
    {
        void Add(TestRun testRun);       
    }
}