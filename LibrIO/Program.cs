using LibrIO.Classes;
using LibrIO.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

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

// Mapper les contrôleurs
app.MapControllers();

// Démarrer l'application
app.Run();

