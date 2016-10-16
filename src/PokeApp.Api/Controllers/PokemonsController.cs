using Microsoft.AspNetCore.Mvc;
using PokeApp.Api.Data;

namespace PokeApp.Api.Controllers
{
    [Route("[controller]")]
    public class PokemonsController
    {
        private readonly IEntityLookup lookup;

        public PokemonsController(IEntityLookup lookup)
        {
            this.lookup = lookup;
        }
        public IActionResult Get()
        {
            var items = lookup.Pokemons;

            return new OkObjectResult(items);
        }
    }
}
