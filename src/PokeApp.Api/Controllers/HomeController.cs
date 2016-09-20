using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace PokeApp.Api.Controllers
{
    public class HomeController
    {
        public static void Get(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                const string logo = @"
  ____       _     __    _                     _    ____ ___ 
 |  _ \ ___ | | __/_/   / \   _ __  _ __      / \  |  _ \_ _|
 | |_) / _ \| |/ / _ \ / _ \ | '_ \| '_ \    / _ \ | |_) | | 
 |  __/ (_) |   <  __// ___ \| |_) | |_) |  / ___ \|  __/| | 
 |_|   \___/|_|\_\___/_/   \_\ .__/| .__/  /_/   \_\_|  |___|
                             |_|   |_|                       ";

                var result = $"{logo}\nAppId: {context.User.Claims.SingleOrDefault(c => c.Type == "appid").Value}";
                await context.Response.WriteAsync(result);
            });
        }
    }
}
