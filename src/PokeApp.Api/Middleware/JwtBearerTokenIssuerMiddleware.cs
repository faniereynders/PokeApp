using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Internal.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PokeApp.Api.Options;
using PokeApp.Api.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace PokeApp.Api.Middleware
{
    public class JwtBearerTokenIssuerMiddleware
    {
        private readonly IConsumerValidator consumerValidator;
        
        private readonly JwtAuthenticationOptions jwtOptions;
        private readonly RequestDelegate next;
        
        public JwtBearerTokenIssuerMiddleware(RequestDelegate next, IOptions<JwtAuthenticationOptions> jwtOptions,
            IConsumerValidator consumerValidator)
        {
            this.next = next;
            this.jwtOptions = jwtOptions.Value;
            this.consumerValidator = consumerValidator;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == jwtOptions.TokenEndpoint &&
                context.Request.Method == "POST" &&
                context.Request.HasFormContentType)
            {
                var headers = (FrameRequestHeaders)context.Request.Headers;
                
                var credentials = Utilities.GetCredentialsFromAuthorizationHeader(headers.HeaderAuthorization.First());

                var valid = consumerValidator.Verify(credentials.Key, credentials.Value);
                if (valid)
                {
                    var accessToken = await Utilities.GenerateToken(credentials.Key, jwtOptions.Parameters.IssuerSigningKey, jwtOptions.Parameters.ValidIssuer);
                    var result = new
                    {
                        token_type = "Bearer",
                        access_token = accessToken
                    };
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
                }
                else
                {
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync("Unable to verify client");
                }
            }
            else
            {
                await next.Invoke(context);
            }
        }
    }
}
