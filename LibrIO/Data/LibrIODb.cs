using LibrIO.Classes;
using Microsoft.EntityFrameworkCore;

namespace LibrIO.Data
{
    public class LibrIODb : DbContext
    {
        public LibrIODb(DbContextOptions<LibrIODb> options) : base(options) { }
        public DbSet<Livre> Livre { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Auteur> Auteur { get; set; }
        public DbSet<Categorie> Categorie { get; set; }
        public DbSet<Membre> Membre { get; set; }
        public DbSet<Employe> Employe { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
