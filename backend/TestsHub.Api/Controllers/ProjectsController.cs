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
        [ResponseCache(Duration = DefaultCachingDuration)]
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
            catch (TesthubApiException)
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
        [ResponseCache(Duration = DefaultCachingDuration)]
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
            catch (TesthubApiException)
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
        [ResponseCache(Duration = DefaultCachingDuration)]
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
        [ResponseCache(Duration = DefaultCachingDuration, VaryByQueryKeys = new string[] { "page", "pageSize", "filter" })]
        public ActionResult<PaginatedList<Data.ProjectSummary>> GetProjects(string org, [FromQuery] int? page, [FromQuery] int? pageSize, [FromQuery] string filter)
        {
            try
            {
                filter ??= string.Empty;

                var repository = RepositoryFactory.GetTestHubDataProvider(org, Url);

                var projects = repository.GetProjects()
                    .Where(p => p.Name.Contains(filter, StringComparison.OrdinalIgnoreCase));
                return PaginatedListBuilder.CreatePaginatedList(projects, page, pageSize, this.Request.Path);
            }
            catch (TesthubApiException)
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
        [ResponseCache(Duration = DefaultCachingDuration, VaryByQueryKeys = new string[] { "runsLimit" })]
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
            catch (TesthubApiException)
            {
                return NotFound();
            }

        }

        /// <summary>
        /// Get badge
        /// </summary>        
        [HttpGet("{project}/badge.svg")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ResponseCache(Duration = 30 * 60 * 60, VaryByQueryKeys = new string[]{"branch"})]
        public ActionResult<Data.TestResultsHistoricalData> GetBadge(string org, string project, [FromQuery] string branch)
        {
            try
            {
                var repository = RepositoryFactory.GetTestHubDataProvider(org, Url);
                var test = repository.GetTestRuns(project, branch).FirstOrDefault();
                var svgContent = string.Empty;
                if (test == null)
                {
                    svgContent = Badge.BadgeGenerator.GenerateBadge(null, 0, null);
                }
                else
                {
                    svgContent = Badge.BadgeGenerator.GenerateBadge(test.Stats.TotalCount, test.Stats.Failed, test.Coverage);
                }
                
                return Content(svgContent, "image/svg+xml; charset=utf-8"); 
             
            }
            catch (TesthubApiException)
            {
                return NotFound();
            }
        }
    }
}
