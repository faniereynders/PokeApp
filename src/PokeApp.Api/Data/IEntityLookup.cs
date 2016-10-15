using PokeApp.Api.Infrastructure;
using PokeApp.Api.Models;
using System;
using System.Collections.Generic;

namespace PokeApp.Api.Data
{
    public interface IEntityLookup
    {
        IEnumerable<Pokemon> Pokemons { get; }
        IEnumerable<Player> Players { get; }
    }
}
