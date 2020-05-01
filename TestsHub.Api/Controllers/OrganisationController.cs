using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestHub.Api.ApiDataProvider;
using TestHub.Api.Controllers.Helpers;

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
        public ActionResult<Data.Organisation> Get(string org)
        {
            var repository = RepositoryFactory.GetTestHubDataProvider(org, Url);
            var orgSummary = repository.GetOrgSummary();

            return FormateResult(orgSummary, $"{org}");
        }

        [HttpGet("{org}/coverage")]
        public ActionResult<IEnumerable<Data.DataItem>> GetCoverage(string org, [FromQuery]int? page, [FromQuery]int? pageSize)
        {
            // retrieve list from database/whereverand
            return Ok(DummyDataProvider.GetDummyCoverage());
        }
        
    }
}
