using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Reflection;

namespace TestHub.Api.ApiDataProvider
{
    public class UrlBuilder
    {
        private readonly IUrlHelper _url;

        public UrlBuilder(IUrlHelper url)
        {
            _url = url;
        }

        public string Action(string action, Type controller, object values)
        {
            var m = controller.GetMethods().FirstOrDefault(m => m.Name.Equals(action, StringComparison.OrdinalIgnoreCase));
            if (m == null)
            {
                throw new InvalidOperationException();
            }
            

            // TODO: add verification for action
            return _url.Action(action, controller.Name.Replace("Controller", "", StringComparison.Ordinal), values);
        }
    }
}
