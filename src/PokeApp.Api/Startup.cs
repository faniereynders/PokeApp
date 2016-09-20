using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;

namespace PokeApp.Api
{
    public class Startup
    {
        private const string defaultIssuer = "http://api.pokeapp.io";
        private readonly static SecurityKey serverKey = new SymmetricSecurityKey(Utilities.Base64UrlDecode("superdupersecretkey123"));

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<IClientValidator, ClientValidator>()
                .AddAuthentication();
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidIssuer = defaultIssuer,
                IssuerSigningKey = serverKey
            };

            app.UseJwtBearerAuthentication(
                authenticationEndpoint: "/jwt/token",
                options: new JwtBearerOptions {
                    TokenValidationParameters = tokenValidationParameters,
                },
                tokenLifeTime: TimeSpan.FromHours(1));

            app.Map("/ping", appContext =>
            {
                appContext.Run(context =>
                {
                    return context.Response.WriteAsync("Pong!");
                });
            });

            app.UseAuthorization();

            //protected resource:
            app.Run(context =>
            {
                const string logo = @"
  ____       _     __    _                     _    ____ ___ 
 |  _ \ ___ | | __/_/   / \   _ __  _ __      / \  |  _ \_ _|
 | |_) / _ \| |/ / _ \ / _ \ | '_ \| '_ \    / _ \ | |_) | | 
 |  __/ (_) |   <  __// ___ \| |_) | |_) |  / ___ \|  __/| | 
 |_|   \___/|_|\_\___/_/   \_\ .__/| .__/  /_/   \_\_|  |___|
                             |_|   |_|                       ";

                var result = $"{logo}\nAppId: {context.User.Claims.SingleOrDefault(c=>c.Type == "appid").Value}";
                return context.Response.WriteAsync(result);
            });
        }

        
        
        

        
        
       
    }
}
