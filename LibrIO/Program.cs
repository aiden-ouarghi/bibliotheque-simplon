using Microsoft.EntityFrameworkCore;
using LibrIO;
using Microsoft.AspNetCore.Components.Forms;
using System.Formats.Tar;
using System;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using LibrIO.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<LibrIODb>(opt => opt.UseSqlite("Data Source=LibriIO.db"));
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

// Ajouter DbContext et configuration de la base de données SQLite
builder.Services.AddDbContext<LibrIODb>(options =>
    options.UseSqlite("Data Source=LibrIO.db"));

// Ajouter les services nécessaires, comme les contrôleurs et Swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "LibrIO API",
        Version = "v1",
        Description = "Une API pour une bibliothèque"
    });
    c.EnableAnnotations();
});

var app = builder.Build(); // Construire l'application après avoir ajouté les services

// Vérifiez si l'environnement est en développement pour activer Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LibrIO API V1");
        c.RoutePrefix = string.Empty;
    });
}


// Initialiser la base de données avec un json (méthode seed)
using (var scope = app.Services.CreateScope())
{
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<LibrIODb>();
        DbInitializer.Seed(dbContext);
    }
}

app.MapControllers();

// Démarrer l'application
app.Run();

