using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;
using PokeApp.Api.Infrastructure;
using PokeApp.Api.Options;
using PokeApp.Api.Validation;
using PokeApp.Api.Controllers;

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
        public void Configure(IApplicationBuilder app)
        {
            app.UseJwtBearerAuthenticationWithTokenIssuer();

            app.Map("/ping", PingController.Get);

            app.UseAuthorization();

            //protected resource:
            app.Map("/", HomeController.Get);
        }
    }
}
