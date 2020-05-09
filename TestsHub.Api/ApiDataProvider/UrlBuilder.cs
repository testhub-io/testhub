using Microsoft.AspNetCore.Mvc;
using System;

namespace TestHub.Api.ApiDataProvider
{
    public class UrlBuilder
    {
        private readonly IUrlHelper _url;

        public UrlBuilder(IUrlHelper url)
        {
            _url = url;
        }

        public string Action(string action, System.Type controller, object values)
        {
            // TODO: add verification for action
            return _url.Action(action, controller.Name.Replace("Controller", "", StringComparison.Ordinal), values);
        }
    }
}
