using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TestHub.Api.ApiDataProvider;

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
            var repository = RepositoryFactory.GetTestHubDataProvider(org, Url);
            var projectData = repository.GetProjectSummary(project);

            return FormateResult(projectData, $"{org}/{project}");
        }

        [HttpGet("testresults")]
        public ActionResult<string> GetTestResults()
        {
            throw new NotImplementedException();            
        }

        [HttpGet("coverage")]
        public ActionResult<string> GetCoverage()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult<string> GetProjects(string org)
        {
            var repository = RepositoryFactory.GetTestHubDataProvider(org, Url);
            var orgSummary = repository.GetProjects(org);

            return FormateResult(orgSummary.ToList(), $"{org}");
        }
    }
}
