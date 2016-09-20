using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PokeApp.Api.Middleware;
using PokeApp.Api.Options;

namespace Microsoft.AspNetCore.Builder
{
    public static class JwtAppBuilderExtensions
    {
        public static IApplicationBuilder UseJwtBearerAuthenticationWithTokenIssuer(this IApplicationBuilder app)
        {
            var jwtOptions = (IOptions<JwtAuthenticationOptions>)app.ApplicationServices.GetService(typeof(IOptions<JwtAuthenticationOptions>));

            var options = new JwtBearerOptions
            {
                TokenValidationParameters = jwtOptions.Value.Parameters
            };

            app.UseJwtBearerAuthentication(options);

            //JWT token endpoint
            app.UseMiddleware<JwtBearerTokenIssuerMiddleware>();

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
