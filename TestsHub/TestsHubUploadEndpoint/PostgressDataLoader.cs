using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using TestsHub.Data.DataModel;


namespace TestsHubUploadEndpoint
{
    public class PostgressDataLoader : IDataLoader
    {
        TestHubDBContext _testHubDBContext = new TestHubDBContext();

        public void Add(TestRun testRun)
        {
            _testHubDBContext.TestRuns.Add(testRun);
            _testHubDBContext.SaveChanges();
        }

        public void Dispose()
        {
            _testHubDBContext.Dispose();
        }
    }
}
