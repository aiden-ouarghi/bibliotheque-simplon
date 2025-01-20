using Microsoft.EntityFrameworkCore;
using LibrIO;
using Microsoft.AspNetCore.Components.Forms;
using System.Formats.Tar;
using System;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<LibrIODb>(opt => opt.UseSqlite("Data Source=LibriIO.db"));
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("V1", new OpenApiInfo
    {
        Title = "LibrIO",
        Version = "V1",
        Description = "Une solution de gestion de catalogue pour les bibliothèquess",
        Contact = new OpenApiContact
        {
            Name = "Julie",
            Email = "dbt.julie@gmail.com",
            Url = new Uri("https://votre-site.com")
        }
    });

    c.EnableAnnotations();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/V1/swagger.json", "LibrIO");
        c.RoutePrefix = ""; 
    });
    app.MapOpenApi();
}

// Initialiser la base de données avec un json (méthode seed)
using (var scope = app.Services.CreateScope())
{
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<LibrIODb>();
        DbInitializer.Seed(dbContext);
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
