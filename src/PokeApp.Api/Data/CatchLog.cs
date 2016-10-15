using System;
using System.Collections.Generic;
using PokeApp.Api.Models;
using System.Linq;

namespace PokeApp.Api.Data
{
    public class CatchLog : ICatchLog
    {
        private readonly IEntityLookup lookup;
        private readonly List<LogEntry> logEntries;
        public CatchLog(IEntityLookup lookup)
        {
            this.lookup = lookup;
            this.logEntries = new List<LogEntry>()
            {
                new LogEntry {CaughtAt = DateTime.Now, Id = 1, PlayerId = 253920080, PokemonId = 1 },
                new LogEntry {CaughtAt = DateTime.Now, Id = 2, PlayerId = 253920080, PokemonId = 2 },
                new LogEntry {CaughtAt = DateTime.Now, Id = 3, PlayerId = 253920080, PokemonId = 3 },
                new LogEntry {CaughtAt = DateTime.Now, Id = 4, PlayerId = 253920080, PokemonId = 4 }
            };
        }

        public Pokemon Add(Pokemon newItem)
        {
            return newItem;
        }

        public LogItem AddEntry(LogEntry entry)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LogItem> GetAllFrom(int fromId, int count)
        {
            var query = from entry in logEntries
                        join pokemon in lookup.Pokemons on entry.PokemonId equals pokemon.Id
                        join player in lookup.Players on entry.PlayerId equals player.Id
                        where entry.PokemonId >= fromId
                        orderby entry.CaughtAt descending
                        select new LogItem
                        {
                            CaughtAt = entry.CaughtAt,
                            CaughtBy = player,
                            Pokemon = pokemon,
                            Id = entry.Id
                        };
            return query.Take(count).ToList();
        }

        
    }
}
