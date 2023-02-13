using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiKeyAuthentication.Authentication
{
    public class ApiKeyEndpointFilter : IEndpointFilter
    {
        public readonly IConfiguration _configuration;

        public ApiKeyEndpointFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context, 
            EndpointFilterDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
            {
                //return TypedResults.Unauthorized("API Key missing");
                return new UnauthorizedHttpObjectResult("API Key missing");
            }

            var apiKey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);            

            if (string.IsNullOrEmpty(apiKey) || !apiKey.Equals(extractedApiKey))
            {
                //return TypedResults.Unauthorized();
                return new UnauthorizedHttpObjectResult("Invalid API Key");
            }

            return await next(context);
        }
    }
}
