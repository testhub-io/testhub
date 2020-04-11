using Microsoft.AspNetCore.Mvc;

namespace TestHub.Api.ApiDataProvider
{
    public class UrlBuilder
    {
        private readonly IUrlHelper _url;

        public UrlBuilder(IUrlHelper url)
        {
            _url = url;
        }

        public string Action(string action, string controller, object values)
        {
            // TODO: add verification for action and controller
            return _url.Action(action, controller, values);
        }
    }
}
