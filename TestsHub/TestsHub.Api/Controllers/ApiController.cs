using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestsHub.Api.Data;
using TestsHub.Data;
using TestsHubUploadEndpoint;

namespace TestsHub.Api.Controllers
{
    [Route("")]
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ApiController : ControllerBase
    {
        IMapper _mapper = Automapper.MapperConfiguration.Mapper;
        IRepositoryFactory RepositoryFactory { get; }

        public ApiController(IRepositoryFactory repositoryFactory)
        {
            RepositoryFactory = repositoryFactory;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{org}/{project}/{testrun}")]
        public ActionResult<string> Get(string org, string project, string testRun)
        {
            var repository = RepositoryFactory.GetTestHubRepository(org);
            var testRunEntity = repository.GetTestRun(project, testRun);            

            return new JsonResult(testRunEntity, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
        }

        // GET api/values/5
        [HttpGet("{org}/{project}")]
        public ActionResult<string> GetProject(string org, string project)
        {
            var repository = RepositoryFactory.GetTestHubRepository(org);
            var projectData = repository.GetProjectSummary(project);

            return new JsonResult(projectData, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
        }

        
        [HttpGet("{org}")]
        public ActionResult<string> GetOrganisation(string org)
        {
            var repository = RepositoryFactory.GetTestHubRepository(org);
            var projectData = repository.GetOrgSummary(org);

            return new JsonResult(projectData, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
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
            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var jUnitReader = new JUnitReader(
                    new DataLoader(repository.TestHubDBContext, project, org));                    
                    var task = jUnitReader.Read(formFile.OpenReadStream(), testRun);
                    Task.WaitAll(task);
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size });
        }

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
