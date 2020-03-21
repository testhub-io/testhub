using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestHub.Data;
using TestsHubUploadEndpoint;
using Microsoft.AspNetCore.Http;

namespace TestHub.Api.Controllers
{
    [Route("")]
    //[Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ApiController : TestHubControllerBase
    {        
        public ApiController(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }


        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            return $"I'm ok. {Assembly.GetExecutingAssembly().GetName().FullName}";
        }
       

        [HttpGet("{org}")]
        public ActionResult<string> GetOrganisation(string org)
        {
            var repository = RepositoryFactory.GetTestHubRepository(org);
            var orgSummary = repository.GetOrgSummary(org);

            return FormateResult(orgSummary, $"{org}");
        }



       

    }
}
