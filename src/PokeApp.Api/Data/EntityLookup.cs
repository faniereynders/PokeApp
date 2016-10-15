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
            pokeomons.Add(new Pokemon { Id = 1, Name = "Pokemon-1" });
            pokeomons.Add(new Pokemon { Id = 2, Name = "Pokemon-2" });
            pokeomons.Add(new Pokemon { Id = 3, Name = "Pokemon-3" });
            pokeomons.Add(new Pokemon { Id = 4, Name = "Pokemon-4" });
            pokeomons.Add(new Pokemon { Id = 5, Name = "Pokemon-5" });
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
