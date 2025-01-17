using System.Collections.Generic;
using LibrIO.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

public class LibrIODb : DbContext
{
    public LibrIODb(DbContextOptions<LibrIODb> options)
    : base(options) { }

    public DbSet<Emprunt> Auteurs => Set<Emprunt>();
    public DbSet<Membre> Membres => Set<Membre>();
    public DbSet<Livre> Livres => Set<Livre>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
}
