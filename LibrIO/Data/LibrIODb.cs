using System.Collections.Generic;
using LibrIO;
using LibrIO.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace LibrIO.Data
{
    public class LibrIODb : DbContext
    {
        public LibrIODb(DbContextOptions<LibrIODb> options)
        : base(options) { }

        public DbSet<Emprunt> Emprunts { get; set; }
        public DbSet<Membre> Membres { get; set; }
        public DbSet<Livre> Livres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Clé composite qui relie la table Emprunt aux tables Livre et Membre ; on aurait pu utiliser deux modelbuilder avec .HasForeignKey 
            modelBuilder.Entity<Emprunt>()
                .HasKey(e => new { e.Id_Livre, e.Id_Membre });
        }

    }
}