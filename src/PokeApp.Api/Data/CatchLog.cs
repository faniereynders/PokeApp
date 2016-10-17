using System;
using System.Collections.Generic;
using PokeApp.Api.Models;
using System.Linq;

namespace PokeApp.Api.Data
{
    public class CatchLog : ICatchLog
    {
        private readonly IEntityLookup lookup;
        private readonly PokeAppDataContext dataContext;

        public CatchLog(IEntityLookup lookup, PokeAppDataContext dataContext)
        {
            this.lookup = lookup;
            this.dataContext = dataContext;
        }

        public Pokemon Add(Pokemon newItem)
        {
            return newItem;
        }

        public LogEntry AddEntry(LogEntry entry)
        {
            var newEntry = dataContext.LogEntries.Add(entry);
            dataContext.SaveChanges();
            return newEntry.Entity;
        }

        public IEnumerable<LogItem> GetAllFrom(int fromId, int count)
        {
            var query = from entry in dataContext.LogEntries
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
