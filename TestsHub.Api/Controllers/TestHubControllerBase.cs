using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHub.Data;

namespace TestHub.Api.Controllers
{
    public abstract class TestHubControllerBase : ControllerBase
    {
        protected IRepositoryFactory RepositoryFactory { get; }

        public TestHubControllerBase(IRepositoryFactory repositoryFactory)
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
