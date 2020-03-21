using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHub.Api.ApiDataProvider;
using TestHub.Commons;
using TestHub.Data;

namespace TestHub.Api.Controllers
{
    public abstract class TestHubControllerBase : ControllerBase
    {

        protected IDataProviderFactory RepositoryFactory { get; }

        protected readonly ILogger Logger = TestHubLogger.CreateLoger<TestHubControllerBase>();

        public TestHubControllerBase(IDataProviderFactory repositoryFactory)
        {
            RepositoryFactory = repositoryFactory;
        }

        protected static ActionResult FormateResult(dynamic data, string request)
        {
            if (data == null)
            {

                return new NotFoundObjectResult(request);
            }
            return new JsonResult(data);
        }
    }
}
