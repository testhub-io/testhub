using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestHub.Api.ApiDataProvider;
using TestHub.Api.Controllers.Helpers;
using TestsHubUploadEndpoint;
using TestsHubUploadEndpoint.Coverage;

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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ResponseCache(Duration = DefaultCachingDuration)]
        public ActionResult<Data.TestRun> Get(string org, string project, string testRun)
        {
            try
            {
                if (string.IsNullOrEmpty(org) || string.IsNullOrEmpty(project) || string.IsNullOrEmpty(testRun))
                {
                    return BadRequest();
                }
                var dataProvider = RepositoryFactory.GetTestHubDataProvider(org, Url);
            
                var testRunEntity = dataProvider.GetTestRunSummary(project, testRun);
                return FormatResult(testRunEntity, $"{org}/{project}/{testRun}");
            }
            catch(TesthubApiException)
            {
                return NotFound();
            }
        }

        [HttpGet("{testrun}/tests")]
        [ResponseCache(Duration = DefaultCachingDuration)]
        public ActionResult<Data.TestRun> GetTests(string org, string project, string testRun)
        {
            try
            {
                if (string.IsNullOrEmpty(org) || string.IsNullOrEmpty(project) || string.IsNullOrEmpty(testRun))
                {
                    return BadRequest();
                }
                var dataProvider = RepositoryFactory.GetTestHubDataProvider(org, Url);

                var testRunEntity = dataProvider.GetTests(project, testRun);
                return FormatResult(testRunEntity, $"{org}/{project}/{testRun}");
            }
            catch(TesthubApiException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get list of test runs for a project. Paginated
        /// </summary>
        /// <param name="org"></param>
        /// <param name="project"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ResponseCache(Duration = DefaultCachingDuration, VaryByQueryKeys = new string[] { "page", "pageSize", "filter" })]
        public ActionResult<PaginatedList<Data.TestRunSummary>> GetTestRuns(string org, string project, [FromQuery]int? page, [FromQuery]int? pageSize, [FromQuery]string filter)
        {
            try
            {
                filter ??= string.Empty;
                var dataProvider = RepositoryFactory.GetTestHubDataProvider(org, Url);
            
                var res = dataProvider.GetTestRuns(project).AsQueryable()
                        .Where(p => p.Name.Contains(filter, StringComparison.OrdinalIgnoreCase));

            
                    return PaginatedListBuilder.CreatePaginatedList(res, page, pageSize, Request.Path);
            }
            catch(TesthubApiException)
            {
                return NotFound();
            }
        }


        /// <summary>
        /// Not implemented 
        /// </summary>      
        [HttpGet("{testrun}/coverage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [ResponseCache(Duration = DefaultCachingDuration)]
        public IActionResult GetCoverage(string org, string project, string testRun)
        {
            var stream = DummyDataProvider.GetDummyTestRunCoverage();

            if (stream == null)
                return NotFound();

            return File(stream, "application/xml"); 
        }

        /// <summary>
        /// Not implemented 
        /// </summary>      
        [HttpGet("{testrun}/code/{**file_path}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]        
        public IActionResult GetSourceCode(string org, string project, string testRun, string filepath)
        {
            var stream = DummyDataProvider.GetDummyTestRunCode();

            if (stream == null)
                return NotFound();

            return File(stream, "text/plain");
        }

        // PUT 
        [HttpPut("{testrun}")]
        [ApiKeyAuth]
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
                if (branch != null)
                {
                    branch = branch.Replace("refs/heads/", "");
                }
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

            if (Request.Form.Files[CoverageKey] == null || Request.Form.Files[CoverageKey].Length <= 0)
                return Ok(new {count = files.Count, size});
            using (var coverageStream = Request.Form.Files[CoverageKey].OpenReadStream()) 
            {
                var factory = new CoverageReaderFactory();
                var coberturaReader = factory.CreateReader(coverageStream, dataLoader);
                coberturaReader.Read(coverageStream, testRun);
            }

            return Ok(new { count = files.Count, size });
        }

        // PUT api/values/5/coverage
        [HttpPut("{testrun}/coverage")]
        [ApiKeyAuth]
        public async Task<ActionResult<string>> PutCoverage(string org, string project, string testRun)
        {
            var repository = RepositoryFactory.GetTestHubWritableDataProvider(org, Url);                        
            var dataLoader = new DataLoader(repository.TestHubDBContext, project, org, testRun);

            await using (var ms = new MemoryStream(2048))
            {
                await Request.Body.CopyToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);
                var factory = new CoverageReaderFactory();
                var coberturaReader = factory.CreateReader(ms, dataLoader);
                coberturaReader.Read(ms, testRun);             
            }
            

            return Ok(new { size = Request.ContentLength });
        }

    }
}
