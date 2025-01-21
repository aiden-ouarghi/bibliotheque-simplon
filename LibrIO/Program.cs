using LibrIO.Classes;
using LibrIO.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LibrIODb>(options =>
    options.UseSqlite("Data Source=LibrIO.db"));

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
var app = builder.Build(); 


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LibrIO API V1");
        c.RoutePrefix = string.Empty;
    });
}


app.MapControllers();

app.Run();

