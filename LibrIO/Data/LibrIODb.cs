using Microsoft.EntityFrameworkCore;
using LibrIO.Classes;

namespace LibrIO.Data
{
    public class LibrIODb : DbContext
    {
        public LibrIODb(DbContextOptions<LibrIODb> options) : base(options) { }
        public DbSet<Livre> Livre { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Auteur> Auteur { get; set; }
        public DbSet<Categorie> Categorie { get; set; }
        public DbSet<Catalogue> Catalogue { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Relation Livre Auteur 
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
            // relation catalogue livre
            modelBuilder.Entity<Livre>()
             .HasOne(livre => livre.Catalogues)  
                .WithOne(catalogue => catalogue.Livre)     
                .HasForeignKey<Catalogue>(catalogue => catalogue.livreId); 
            
        }
    }
}
