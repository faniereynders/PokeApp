using Microsoft.EntityFrameworkCore;
using PokeApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokeApp.Api.Data
{
    public class PokeAppDataContext : DbContext
    {
        public PokeAppDataContext(DbContextOptions<PokeAppDataContext> options)
            : base(options)
        { }

        public DbSet<LogEntry> LogEntries { get; set; }
    }
}
