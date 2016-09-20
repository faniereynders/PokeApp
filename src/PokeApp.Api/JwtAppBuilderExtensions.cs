using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Internal.Http;
using Newtonsoft.Json;
using PokeApp.Api;
using System.Linq;

namespace Microsoft.AspNetCore.Builder
{
    public static class JwtAppBuilderExtensions
    {
        public static IApplicationBuilder UseJwtBearerAuthentication(this IApplicationBuilder app, PathString authenticationEndpoint, JwtBearerOptions options)
        {
            app.UseJwtBearerAuthentication(options);

            //JWT token endpoint
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == authenticationEndpoint &&
                context.Request.Method == "POST" &&
                context.Request.HasFormContentType)
                {
                    var headers = (FrameRequestHeaders)context.Request.Headers;
                    var validator = (IConsumerValidator)app.ApplicationServices.GetService(typeof(IConsumerValidator));
                    
                    var credentials = Utilities.GetCredentialsFromAuthorizationHeader(headers.HeaderAuthorization.First());

                    var valid = validator.Verify(credentials.Key, credentials.Value);
                    if (valid)
                    {
                        var accessToken = await Utilities.GenerateToken(credentials.Key, options.TokenValidationParameters.IssuerSigningKey, options.TokenValidationParameters.ValidIssuer);
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
                    await next();
                }
            });

            return app;
        }

        public static IApplicationBuilder UseAuthorization(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    await next();
                }
                else
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid token");
                }
            });

            return app;
        }
    }
}
