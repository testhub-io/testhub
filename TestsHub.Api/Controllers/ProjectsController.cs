using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TestHub.Api.ApiDataProvider;
using TestHub.Data.DataModel;
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
        public ActionResult<string> Get(string org, string project)
        {
            var repository = RepositoryFactory.GetTestHubDataProvider(org, Url);
            var projectData = repository.GetProjectSummary(project);
            // FormateResult(DummyDataProvider.GetDummyProjectSummary(org, project, new UrlBuilder(Url)), $"{org}/{project}");
            return Ok(projectData);
        }

        /// <summary>
        /// Get historical test results series
        /// </summary>        
        [HttpGet("{project}/testresults")]        
        public ActionResult<Data.TestResultsHistoricalData> GetTestResults(string org, string project)
        {
            var repository = RepositoryFactory.GetTestHubDataProvider(org, Url);
            var testResultsSeries = repository.GetTestResultsForProject(project);

            return Ok(testResultsSeries);
        }


        /// <summary>
        /// Get historical coverage data for project
        /// </summary>        
        [HttpGet("{project}/coverage")]
        public ActionResult<Data.CoverageHistoricalData> GetProjectCoverage(string org, string project)
        {
            var repository = RepositoryFactory.GetTestHubDataProvider(org, Url);
            var coverage = repository.GetCoverageHistory(project);

            return Ok(coverage);
        }

        [HttpGet]
        public ActionResult<PaginatedList<Data.ProjectSummary>> GetProjects(string org, [FromQuery]int? page, [FromQuery]int? pageSize, [FromQuery]string filter)
        {
            if (filter == null)
                filter = string.Empty;

            var repository = RepositoryFactory.GetTestHubDataProvider(org, Url);
            var projects = repository.GetProjects().Where(p=>p.Name.Contains(filter, StringComparison.OrdinalIgnoreCase));

            return PaginatedListBuilder.CreatePaginatedList(projects, page, pageSize, this.Request.Path);
        }    
    }
}
