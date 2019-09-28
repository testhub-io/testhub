using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestsHub.Api.Data;

namespace TestsHub.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ApiController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{org}/{project}/{testrun}")]
        public ActionResult<string> Get(string org, string project, int testRun)
        {
            var testRunData = new TestRun()
            {
                TestCases = new List<TestCase>()
             {
                 new TestCase() { Status = "Success", Name = "test_dm" }
             },
                Id = "2"
            };
            return new JsonResult(testRunData, new JsonSerializerSettings()
            { Formatting = Formatting.Indented });
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
