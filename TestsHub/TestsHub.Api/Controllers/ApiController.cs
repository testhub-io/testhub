using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestsHub.Api.Data;
using TestsHub.Data;

namespace TestsHub.Api.Controllers
{
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

            var testRunDto = _mapper.Map<Data.TestRun>(testRunEntity);

            return new JsonResult(testRunDto, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
