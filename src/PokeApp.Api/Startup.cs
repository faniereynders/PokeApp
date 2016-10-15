using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PokeApp.Api.Data;
using PokeApp.Api.Infrastructure;
using PokeApp.Api.Options;
using PokeApp.Api.Validation;

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
                .AddSingleton<IConfigureOptions<JwtAuthenticationOptions>, JwtAuthenticationOptionsConfiguration>()
                .AddSingleton<IConsumerValidator, ConsumerValidator>()
                .AddSingleton(configuration)
                .AddSingleton<IEntityLookup, EntityLookup>()
                .AddTransient<ICatchLog, CatchLog>();

            services
                .AddMvcCore(options =>
                {
                    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                    options.Filters.Add(new AuthorizeFilter(policy));                   
                    options.OutputFormatters.Insert(0, new PingPongOutputFormatter());
                })
                .AddAuthorization()
                .AddJsonFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseJwtBearerAuthenticationWithTokenIssuer();

            app.UseMvc();
        }
    }
}
