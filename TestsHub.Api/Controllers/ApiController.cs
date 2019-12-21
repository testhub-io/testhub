﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        IRepositoryFactory RepositoryFactory { get; }

        public ApiController(IRepositoryFactory repositoryFactory)
        {
            RepositoryFactory = repositoryFactory;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            return new string[] { "I'm ok" };
        }

        // GET api/values/5
        [HttpGet("{org}/{project}/{testrun}")]
        public ActionResult<string> Get(string org, string project, string testRun)
        {
            if (string.IsNullOrEmpty(org) || string.IsNullOrEmpty(project) || string.IsNullOrEmpty(testRun))
            {

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

    }
}
