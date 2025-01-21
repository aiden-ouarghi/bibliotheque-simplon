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
        public LibrIODb(DbContextOptions<LibrIODb> options) : base(options) { }
        public DbSet<Livre> Livre { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Auteur> Auteur { get; set; }
        public DbSet<Categorie> Categorie { get; set; }
        public DbSet<Emprunt> Emprunt { get; set; }
        public DbSet<Membre> Membre { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Livre>()
             .HasOne(livre => livre.Auteur)
             .WithMany(auteur => auteur.Livre)
             .HasForeignKey(livre => livre.AuteurId);

            // relation Livre Categorie 
            modelBuilder.Entity<Livre>()
                .HasOne(livre => livre.Categorie)
                .WithMany(categorie => categorie.Livre)
                .HasForeignKey(livre => livre.CategorieId);

            // relation Livre Genre 
            modelBuilder.Entity<Livre>()

             // si je veut HasMany transformer en liste car c'est possible sur ce cas un livre pourrais avoir plusieur genre
             .HasOne(livre => livre.Genre)
             .WithMany(genre => genre.Livres)
             .HasForeignKey(livre => livre.GenreId);           

            // Configuration de Emprunt
            modelBuilder.Entity<Emprunt>()
                .Property(e => e.Id);

            modelBuilder.Entity<Emprunt>()
                .HasOne(e => e.Livre)
                .WithMany(l => l.Emprunt)
                .HasForeignKey(e => e.Id_Livre);

            modelBuilder.Entity<Emprunt>()
                .HasOne(e => e.Membre)
                .WithMany(m => m.Emprunt)
                .HasForeignKey(e => e.Id_Membre);

        }
    }
}
