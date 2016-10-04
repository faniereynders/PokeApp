using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace PokeApp.Api.Controllers
{
    public class HomeController
    {
        [ActionContext]
        public ActionContext ActionContext { get; set; }

        [Route("")]
        public IActionResult Get()
        {
            const string logo = @"
  ____       _     __    _                     _    ____ ___ 
 |  _ \ ___ | | __/_/   / \   _ __  _ __      / \  |  _ \_ _|
 | |_) / _ \| |/ / _ \ / _ \ | '_ \| '_ \    / _ \ | |_) | | 
 |  __/ (_) |   <  __// ___ \| |_) | |_) |  / ___ \|  __/| | 
 |_|   \___/|_|\_\___/_/   \_\ .__/| .__/  /_/   \_\_|  |___|
                             |_|   |_|                       ";

            var result = $"{logo}\nAppId: {ActionContext.HttpContext.User.Claims.SingleOrDefault(c => c.Type == "appid").Value}";
            return new OkObjectResult(result);
        }
    }
}
