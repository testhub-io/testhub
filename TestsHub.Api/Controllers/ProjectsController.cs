using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TestHub.Api.ApiDataProvider;
using TestHub.Api.Controllers.Helpers;

namespace TestHub.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/{org}/[controller]")]
    public class ProjectsController : TestHubControllerBase
    {
        public ProjectsController(IDataProviderFactory repositoryFactory) : base(repositoryFactory)
        {
        }

        /// <summary>
        /// Get project summary
        /// </summary>    
        [HttpGet("{project}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult<string> Get(string org, string project)
        {
            try
            {
                var repository = RepositoryFactory.GetTestHubDataProvider(org, Url);
                var projectData = repository.GetProjectSummary(project);
                if (projectData == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(projectData);
                }
            } 
            catch(TesthubApiException)
            {
                return NotFound();
            }

        }

        /// <summary>
        /// Get historical test results series
        /// </summary>        
        [HttpGet("{project}/testresults")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult<Data.TestResultsHistoricalData> GetTestResults(string org, string project)
        {
            try
            {
                var repository = RepositoryFactory.GetTestHubDataProvider(org, Url);
                var testResultsSeries = repository.GetTestResultsForProject(project);

                if (testResultsSeries == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(testResultsSeries);
                }
            }  
            catch(TesthubApiException)
            {
                return NotFound();
            }
        }


        /// <summary>
        /// Get historical coverage data for project
        /// </summary>        
        [HttpGet("{project}/coverage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult<Data.CoverageHistoricalData> GetProjectCoverage(string org, string project)
        {
            try
            {
                var repository = RepositoryFactory.GetTestHubDataProvider(org, Url);
                var coverage = repository.GetCoverageHistory(project);

                if (coverage == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(coverage);
                }
            }
            catch (TesthubApiException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult<PaginatedList<Data.ProjectSummary>> GetProjects(string org, [FromQuery]int? page, [FromQuery]int? pageSize, [FromQuery]string filter)
        {
            try
            {
                filter ??= string.Empty;

                var repository = RepositoryFactory.GetTestHubDataProvider(org, Url);
            
                var projects = repository.GetProjects()
                    .Where(p => p.Name.Contains(filter, StringComparison.OrdinalIgnoreCase));
                return PaginatedListBuilder.CreatePaginatedList(projects, page, pageSize, this.Request.Path);
            }
            catch(TesthubApiException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get tests analytics for project
        /// </summary>        
        [HttpGet("{project}/tests")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult<Data.TestResultsHistoricalData> GetTest(string org, string project, [FromQuery] int? runsLimit)
        {
            try
            {
                var repository = RepositoryFactory.GetTestHubDataProvider(org, Url);

                if (!runsLimit.HasValue)
                {
                    runsLimit = 45;
                }

                var testResultsSeries = repository.GetTestGrid(project, runsLimit.Value);
                return Ok(testResultsSeries);
            }
            catch(TesthubApiException)
            {
                return NotFound();
            }
            
        }
    }
}
