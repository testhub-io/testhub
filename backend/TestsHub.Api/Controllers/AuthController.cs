using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TestHub.Api.ApiDataProvider;
using Microsoft.AspNetCore.Http;

namespace TestHub.Api.Controllers
{
    [Route("auth")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : TestHubControllerBase
    {
        private static readonly HttpClient client = new HttpClient();

        public AuthController(IDataProviderFactory repositoryFactory, IConfiguration configuration) : base(repositoryFactory)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [HttpGet("github")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<string>> Get([FromQuery]  string token)
        {
            var clientId = Configuration["GithubAppClientId"];
            var clientSecret = Configuration["GithubAppClientSecret"];


            var values = new Dictionary<string, string>
            {
                { "client_id", clientId},
                { "client_secret", clientSecret },
                {"code",token }
            };

            var content = new FormUrlEncodedContent(values);
            client.DefaultRequestHeaders.Add("accept", "application/json");
            var response = await client.PostAsync("https://github.com/login/oauth/access_token", content);

            var responseString = await response.Content.ReadAsStringAsync();
            
            var d = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);
            const string tokenName = "access_token";
            if (d.ContainsKey(tokenName))
            {
                return Ok( new { token = responseString } );
            }else
            {
                return BadRequest(d);
            }
        }
    }
}
