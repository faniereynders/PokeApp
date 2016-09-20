using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;

namespace PokeApp.Api
{
    public class Startup
    {
        private readonly IConfigurationRoot configuration;
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddIniFile("consumers.ini");

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();

            configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddOptions()
                .Configure<ConsumerOptions>(configuration.GetSection("Consumers"))
                .AddAuthentication()
                .AddSingleton<IConfigureOptions<JwtAuthenticationOptions>, JwtAuthenticationOptionsConfiguration>()
                .AddSingleton<IConsumerValidator, ConsumerValidator>()
                .AddSingleton(configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IOptions<JwtAuthenticationOptions> jwtOptions)
        {
            app.UseJwtBearerAuthentication(
                authenticationEndpoint: jwtOptions.Value.TokenEndpoint,
                options: new JwtBearerOptions {
                    TokenValidationParameters = jwtOptions.Value.Parameters,
                });

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
