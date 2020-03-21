using Microsoft.AspNetCore.Mvc;
using TestHub.Api.ApiDataProvider;

namespace TestHub.Api.Controllers
{
    [ApiController]
    [Route("{org}/[controller]")]    
    [Produces("application/json")]
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
    }
}
