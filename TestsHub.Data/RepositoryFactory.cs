using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TestHub.Data.DataModel;

namespace TestHub.Data
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IConfiguration _configuration;

        public RepositoryFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ITestHubRepository GetTestHubRepository(string organisation)
        {
            var context = new TestHubDBContext(_configuration);            
            return new TestHubRepository(context, organisation);

        }
    }
}
