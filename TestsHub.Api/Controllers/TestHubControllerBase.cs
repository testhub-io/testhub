using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestHub.Api.ApiDataProvider;
using TestHub.Commons;

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

        protected static ActionResult FormatResult(dynamic data, string request)
        {
            if (data == null)
            {
                return new NotFoundObjectResult(request);
            }
            return new JsonResult(data);
        }
    }
}
