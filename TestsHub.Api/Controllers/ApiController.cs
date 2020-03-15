using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestsHub.Data;
using TestsHubUploadEndpoint;
using Microsoft.AspNetCore.Http;

namespace TestsHub.Api.Controllers
{
    [Route("")]
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ApiController : ControllerBase
    {
        private const string CoverageKey = "coverage";

        IRepositoryFactory RepositoryFactory { get; }

        public ApiController(IRepositoryFactory repositoryFactory)
        {
            RepositoryFactory = repositoryFactory;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            return $"I'm ok. {Assembly.GetExecutingAssembly().GetName().FullName}";
        }

        // GET api/values/5
        [HttpGet("{org}/{project}/{testrun}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public ActionResult<string> Get(string org, string project, string testRun)
        {
            if (string.IsNullOrEmpty(org) || string.IsNullOrEmpty(project) || string.IsNullOrEmpty(testRun))
            {
                return BadRequest();
            }
            var repository = RepositoryFactory.GetTestHubRepository(org);
            var testRunEntity = repository.GetTestRun(project, testRun);

            return FormateResult(testRunEntity, $"{org}/{project}/{testRun}");
        }

        // GET api/values/5
        [HttpGet("{org}/{project}")]
        public ActionResult<string> GetProject(string org, string project)
        {
            var repository = RepositoryFactory.GetTestHubRepository(org);
            var projectData = repository.GetProjectSummary(project);

            return FormateResult(projectData, $"{org}/{project}");
        }

        private static ActionResult FormateResult(dynamic data, string request)
        {
            if (data == null)
            {

                return new NotFoundObjectResult(request);
            }
            return new JsonResult(data);
        }

        [HttpGet("{org}")]
        public ActionResult<string> GetOrganisation(string org)
        {
            var repository = RepositoryFactory.GetTestHubRepository(org);
            var orgSummary = repository.GetOrgSummary(org);

            return FormateResult(orgSummary, $"{org}");
        }

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/values/5
        [HttpPut("{org}/{project}/{testrun}")]
        public ActionResult<string> Put(string org, string project, string testRun)
        {
            var repository = RepositoryFactory.GetTestHubRepository(org);
            var files = Request.Form.Files; 
            var size = files.Sum(f => f.Length);
            var dataLoader = new DataLoader(repository.TestHubDBContext, project, org);
            var branch = "<missing>";
            var commitId = string.Empty;

            if (Request.Form.ContainsKey("branch"))
            {
                branch = Request.Form["branch"].ToString();
            }

            if (Request.Form.ContainsKey("commitId"))
            {
                commitId = Request.Form["commitId"].ToString();
            }

            foreach (var formFile in files)
            {                         
                if (!formFile.Name.Equals(CoverageKey, System.StringComparison.OrdinalIgnoreCase) && formFile.Length > 0)
                {
                    var jUnitReader = new JUnitReader(dataLoader);

                    var task = jUnitReader.Read(formFile.OpenReadStream(), testRun, branch, commitId);
                    Task.WaitAll(task);
                }
            }

            if (Request.Form.Files[CoverageKey] != null && Request.Form.Files[CoverageKey].Length > 0)
            {
                var coberturaReader = new CoberturaReader(dataLoader);
                coberturaReader.Read(Request.Form.Files[CoverageKey].OpenReadStream(), testRun);
            }          

            return Ok(new { count = files.Count, size });
        }

    }
}
