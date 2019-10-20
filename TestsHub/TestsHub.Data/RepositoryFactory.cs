using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestsHub.Data
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IConfiguration _configuration;

        public RepositoryFactory (IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public ITestHubRepository GetTestHubRepository(string organisation)
        {
            return new TestHubRepository(new DataModel.TestHubDBContext(_configuration), organisation);
        }
    }
}
