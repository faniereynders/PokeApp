using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace PokeApp.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                if (context.Request.Headers["API-KEY"] == "key123")
                {
                    await next();
                }
                else
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("No access");
                }
                
            });

            app.Run(context =>
            {
                const string logo = @"
  ____       _     __    _                     _    ____ ___ 
 |  _ \ ___ | | __/_/   / \   _ __  _ __      / \  |  _ \_ _|
 | |_) / _ \| |/ / _ \ / _ \ | '_ \| '_ \    / _ \ | |_) | | 
 |  __/ (_) |   <  __// ___ \| |_) | |_) |  / ___ \|  __/| | 
 |_|   \___/|_|\_\___/_/   \_\ .__/| .__/  /_/   \_\_|  |___|
                             |_|   |_|                       ";

                return context.Response.WriteAsync(logo);
            });
        }
    }
}
