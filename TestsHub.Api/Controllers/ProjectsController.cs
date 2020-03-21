using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHub.Api.Controllers;
using TestHub.Data;

namespace TestsHub.Api.Controllers
{
    [ApiController]
    [Route("{org}/[controller]")]    
    [Produces("application/json")]
    public class ProjectsController : TestHubControllerBase
    {
        public ProjectsController(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
        
        [HttpGet("{project}")]
        public ActionResult<string> Get(string org, string project)
        {
            var repository = RepositoryFactory.GetTestHubRepository(org);
            var projectData = repository.GetProjectSummary(project);

            return FormateResult(projectData, $"{org}/{project}");
        }
    }
}
