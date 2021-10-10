using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Octokit;
using Octokit.Internal;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace TestHub.Integration.Github
{
    public class GithubApi
    {
        private const string GithubApplicationId = "67825";
        const string InstallationId = "9684141"; // from webhook payload
        private const string ProductHeaderValue = "Testhub";

        private string getJwtToken()
        {
            var rsaXml = string.Empty;
            using (var stream = typeof(GithubApi).Assembly.GetManifestResourceStream("TestHub.Integration.Github.test-hub-io.2020-06-11.private-key.pem.xml"))
            {
                var reader = new StreamReader(stream);
                rsaXml = reader.ReadToEnd();
            }

            var rsaProvider = new RSACryptoServiceProvider();
            rsaProvider.FromXmlString(rsaXml);
            var key = new RsaSecurityKey(rsaProvider);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateJwtSecurityToken(GithubApplicationId, null, null, null, DateTime.Now.AddMinutes(5),
                DateTime.Now, new SigningCredentials(key, SecurityAlgorithms.RsaSha256));

            return token.RawData;

        }

        private async Task<string> GenerateInstallationToken(string installationId)
        {
            var httpClient = new HttpClient();
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"https://api.github.com/installations/{installationId}/access_tokens");
            requestMessage.Headers.Add("User-Agent", "Static-Scheduler");
            requestMessage.Headers.Add("Authorization", $"Bearer {getJwtToken()}");
            requestMessage.Headers.Add("Accept", "application/vnd.github.machine-man-preview+json");
            var response = await httpClient.SendAsync(requestMessage);

            var content = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<dynamic>(content);

            return token.token;
        }

        public async Task<string> GetThing()
        {           
            var token = getJwtToken();
            var instToken = await GenerateInstallationToken(InstallationId);
            var github = new GitHubClient(new ProductHeaderValue(ProductHeaderValue), new InMemoryCredentialStore(new Credentials(instToken)));
            

            var t = github.Repository.Get(191186411);
            Task.WaitAll(t);
            return t.Result.CloneUrl;
        }
       
    }
}
