using System.Collections.Generic;
using System.Threading.Tasks;
using PokeApp.Api.Models;

namespace PokeApp.Api.Data
{
    public class EntityLookup : IEntityLookup
    {
        private readonly List<Player> players;
        private readonly List<Pokemon> pokeomons;

        public EntityLookup()
        {
            this.players = new List<Player>();
            this.pokeomons = new List<Pokemon>();

            Task.WaitAll(populatePlayers(), populatePokemons());
        }

        private async Task populatePlayers()
        {
            players.Add(new Player { Id = 253920080, Name = "Fanie" });
            await Task.FromResult(0);
        }
        private async Task populatePokemons()
        {
            pokeomons.Add(new Pokemon { Id = 1, Name = "Bulbasaur" });
            pokeomons.Add(new Pokemon { Id = 2, Name = "Ivysaur" });
            pokeomons.Add(new Pokemon { Id = 3, Name = "Venusaur" });
            pokeomons.Add(new Pokemon { Id = 4, Name = "Charmander" });
            pokeomons.Add(new Pokemon { Id = 5, Name = "Charmeleon" });
            await Task.FromResult(0);
        }

        public IEnumerable<Player> Players
        {
            get
            {
                return this.players;
            }
        }

        public IEnumerable<Pokemon> Pokemons
        {
            get
            {
                return this.pokeomons;
            }
        }
    }
}
