using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace PokeApp.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddJwtAuthentication();
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseJwtAuthentication();

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
