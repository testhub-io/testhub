using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using TestHub.Api.ApiDataProvider;

namespace TestHub.Api.Controllers
{
    //[Route("")]
    [Route("api")]
    [ApiController]
    [Produces("application/json")]
    public class OrganisationController : TestHubControllerBase
    {
        public OrganisationController(IDataProviderFactory repositoryFactory) : base(repositoryFactory)
        {
        }

        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            return $"I'm ok. {Assembly.GetExecutingAssembly().GetName().FullName}";
        }

        [HttpGet("{org}")]
        public ActionResult<string> Get(string org)
        {
            var repository = RepositoryFactory.GetTestHubDataProvider(org, Url);
            var orgSummary = repository.GetOrgSummary();

            return FormateResult(orgSummary, $"{org}");
        }

        [HttpGet("{org}/coverage")]
        public ActionResult<string> GetCoverage()
        {
            throw new NotImplementedException();
        }

    }
}
