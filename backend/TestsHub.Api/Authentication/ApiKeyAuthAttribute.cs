using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using TestHub.Api.Authentication;

namespace TestHub.Api
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string ApiKeyHeaderName = "ApiToken";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
                                    
            if (!ApiKeyValidator.IsKeyValid(potentialApiKey, context.RouteData.Values["org"].ToString()))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }

    }



}
