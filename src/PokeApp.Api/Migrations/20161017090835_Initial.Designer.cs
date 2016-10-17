using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using PokeApp.Api.Data;

namespace PokeApp.Api.Migrations
{
    [DbContext(typeof(PokeAppDataContext))]
    [Migration("20161017090835_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PokeApp.Api.Models.LogEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CaughtAt");

                    b.Property<int>("PlayerId");

                    b.Property<int>("PokemonId");

                    b.HasKey("Id");

                    b.ToTable("LogEntries");
                });
        }
    }
}
