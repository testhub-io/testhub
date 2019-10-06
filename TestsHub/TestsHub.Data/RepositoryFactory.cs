using System;
using System.Collections.Generic;
using System.Text;

namespace TestsHub.Data
{
    public class RepositoryFactory
    {
        public static ITestHubRepository GetTestHubRepository(string organisation)
        {
            return new TestHubRepository(new DataModel.TestHubDBContext(), organisation);
        }
    }
}
