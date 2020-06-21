﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestHub.Api.ApiDataProvider;
using TestHub.Api.Controllers.Helpers;
using TestsHubUploadEndpoint;

namespace TestHub.Api.Controllers
{
    [ApiController]
    [Route("api/{org}/projects/{project}/runs")]
    [Produces("application/json")]
    public class TestRunsController : TestHubControllerBase
    {
        private const string CoverageKey = "coverage";

        public TestRunsController(IDataProviderFactory repositoryFactory) : base(repositoryFactory)
        {
        }

        [HttpGet("{testrun}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]        
        public ActionResult<Data.TestRun> Get(string org, string project, string testRun)
        {
            if (string.IsNullOrEmpty(org) || string.IsNullOrEmpty(project) || string.IsNullOrEmpty(testRun))
            {
                return BadRequest();
            }
            var dataProvider = RepositoryFactory.GetTestHubDataProvider(org, Url);
            var testRunEntity = dataProvider.GetTestRunSummary(project, testRun);

            return FormateResult(testRunEntity, $"{org}/{project}/{testRun}");
        }

        [HttpGet("{testrun}/tests")]        
        public ActionResult<Data.TestRun> GetTests(string org, string project, string testRun)
        {
            if (string.IsNullOrEmpty(org) || string.IsNullOrEmpty(project) || string.IsNullOrEmpty(testRun))
            {
                return BadRequest();
            }
            var dataProvider = RepositoryFactory.GetTestHubDataProvider(org, Url);
            var testRunEntity = dataProvider.GetTests(project, testRun);

            return FormateResult(testRunEntity, $"{org}/{project}/{testRun}");
        }

        /// <summary>
        /// Get list of test runs for a project. Paginated
        /// </summary>
        /// <param name="org"></param>
        /// <param name="project"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<PaginatedList<Data.TestRunSummary>> GetTestRuns(string org, string project, [FromQuery]int? page, [FromQuery]int? pageSize)
        {            
            var dataProvider = RepositoryFactory.GetTestHubDataProvider(org, Url);
            var res = dataProvider.GetTestRuns(project).AsQueryable();
            return PaginatedListBuilder.CreatePaginatedList(res, page, pageSize, this.Request.Path); 
        }


        /// <summary>
        /// Not implemented 
        /// </summary>      
        [HttpGet("{testrun}/coverage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]  
        public IActionResult GetCoverage(string org, string project, string testRun)
        {
            var stream = DummyDataProvider.GetDummyTestRunCoverage();

            if (stream == null)
                return NotFound();

            return File(stream, "application/xml"); 
        }

        // PUT api/values/5
        [HttpPut("{testrun}")]
        public ActionResult<string> Put(string org, string project, string testRun)
        {
            var repository = RepositoryFactory.GetTestHubWritableDataProvider(org, Url);
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
