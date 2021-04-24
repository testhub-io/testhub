using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestHub.Api.ApiDataProvider;
using TestHub.Api.Authentication;
using TestHub.Api.Controllers.Helpers;
using Microsoft.AspNetCore.Http;

namespace TestHub.Api.Controllers
{    
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ResponseCache(Duration=DefaultCachingDuration)]
        public ActionResult<Data.Organisation> Get(string org)
        {
            try
            {
                var repository = RepositoryFactory.GetTestHubDataProvider(org, Url);
                var orgSummary = repository.GetOrgSummary();
                return FormatResult(orgSummary, $"{org}");
            }
            catch (TesthubApiException)
            {
                return NotFound();
            }

        }

        [HttpGet("{org}/apikey")]
        public ActionResult<Data.Organisation> Get(string org, [FromHeader] string token)
        {
            if (token != null && token.Equals("WinLost2020$")){
                return Ok(ApiKeyValidator.GenerateApiKey(org));
            }

            return Forbid();
        }

        [HttpGet("{org}/coverage")]
        [ResponseCache(Duration = DefaultCachingDuration)]
        public ActionResult<IEnumerable<Data.CoverageDataItem>> GetCoverage(string org, [FromQuery]int? page, [FromQuery]int? pageSize)
        {
            // retrieve list from database/whereverand
            throw new NotImplementedException();
        }
 

        /// <summary>
        /// Get historical test results series
        /// </summary>        
        [HttpGet("{org}/testresults")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ResponseCache(Duration = DefaultCachingDuration)]
        public ActionResult<Data.TestResultsHistoricalData> GetTestResults(string org)
        {
            try
            {
                var repository = RepositoryFactory.GetTestHubDataProvider(org, Url);
                var testResultsSeries = repository.GetTestResultsForOrganisation();
                return Ok(testResultsSeries);
            }
            catch (TesthubApiException)
            {
                return NotFound();
            }
        }

    }
}
