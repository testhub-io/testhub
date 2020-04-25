using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TestHub.Api.ApiDataProvider;
using TestHub.Data.DataModel;

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

        [HttpGet("{project}")]
        public ActionResult<string> Get(string org, string project)
        {
            //var repository = RepositoryFactory.GetTestHubDataProvider(org, Url);
            //var projectData = repository.GetProjectSummary(project);

            return FormateResult(DummyDataProvider.GetDummyProjectSummary(org, project, new UrlBuilder(Url)), $"{org}/{project}");
        }

        [HttpGet("testresults")]
        public ActionResult<string> GetTestResults()
        {
            throw new NotImplementedException();
        }

        [HttpGet("coverage")]
        public ActionResult<Data.CoverageHistoricalData> GetCoverage()
        {
            return DummyDataProvider.GetDummyCoverage();  
        }

        [HttpGet]
        public ActionResult<IEnumerable<Data.ProjectSummary>> GetProjects(string org)
        {
            var repository = RepositoryFactory.GetTestHubDataProvider(org, Url);
            var orgSummary = repository.GetProjects();
            return FormateResult(orgSummary.ToList(), $"{org}");
        }    
    }
}
