using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestHub.Data;
using TestsHubUploadEndpoint;
using Microsoft.AspNetCore.Http;
using TestHub.Api.ApiDataProvider;
using TestHub.Commons;
using Microsoft.Extensions.Logging;

namespace TestHub.Api.Controllers
{
    [Route("")]
    //[Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrganisatioController : TestHubControllerBase
    {        
        public OrganisatioController(IDataProviderFactory repositoryFactory) : base(repositoryFactory)
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
            var orgSummary = repository.GetOrgSummary(org);
            
            return FormateResult(orgSummary, $"{org}");
        }       
    }
}
