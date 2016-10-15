using Microsoft.AspNetCore.Mvc;
using PokeApp.Api.Data;
using PokeApp.Api.Models;

namespace PokeApp.Api.Controllers
{
    [Route("[controller]")]
    public class FeedController : ControllerBase
    {
        private readonly ICatchLog catchLog;
        private const int defaultMaxItemCount = 5;

        public FeedController(ICatchLog catchLog)
        {
            this.catchLog = catchLog;
        }

        [HttpGet("")]
        [HttpGet("from/{fromId:int}")]
        [HttpGet("from/{fromId:int}/take/{limit:int}")]
        public IActionResult Get(int fromId = 0, int limit = defaultMaxItemCount)
        {
            var items = catchLog.GetAllFrom(fromId, limit);
            return Ok(items);
        }

        public IActionResult Post([FromBody]LogEntry newEntry)
        {
            var entry = catchLog.AddEntry(newEntry);
            return Ok(entry);
        }
    }
}
