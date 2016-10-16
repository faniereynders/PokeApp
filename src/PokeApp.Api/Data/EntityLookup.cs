using System.Collections.Generic;
using System.Threading.Tasks;
using PokeApp.Api.Models;
using AngleSharp;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

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
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"https://api.meetup.com/dutchdotnet/events/233731810/rsvps?&sign=true&photo-host=public&response=yes&only=member");
                var content = await response.Content.ReadAsStringAsync();
                var meetupRsvps = JsonConvert.DeserializeObject<dynamic[]>(content);

                var items = meetupRsvps.Select(i => new Player {
                    Id = i.member.id,
                    Name = i.member.name,
                    ImageUrl = i.member.photo?.thumb_link
                });

                players.AddRange(items);
            }
            await Task.FromResult(0);
        }
        private async Task populatePokemons()
        {
            pokeomons.Clear();
            var config = Configuration.Default.WithDefaultLoader();
            var address = "http://pokemondb.net/pokedex/all";
            var document = await BrowsingContext.New(config).OpenAsync(address);
            var rows = document.QuerySelectorAll("#pokedex tbody tr");
            var items = rows.Select(m => new Pokemon
            {
                Id = int.Parse(m.QuerySelector(".num")?.TextContent.Trim()),
                Name = m.QuerySelector(".ent-name")?.TextContent
            }).Distinct();

            pokeomons.AddRange(items);

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
