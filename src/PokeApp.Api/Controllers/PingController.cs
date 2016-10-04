using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PokeApp.Api.Controllers
{
    [AllowAnonymous]
    public class PingController
    {
        [Route("/ping")]
        public IActionResult Get()
        {
            return new OkObjectResult("Pong!");
        }
    }
}
