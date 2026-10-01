using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins("https://localhost:7102", "http://localhost:5048") // Ports vom Blazor-Projekt, anpassen
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var cs = builder.Configuration.GetConnectionString("Default")!;
builder.Services.AddDbContext<AppDbContext>(o => o.UseMySql(cs, ServerVersion.AutoDetect(cs)));
builder.Services.AddControllers();

var app = builder.Build();
app.UseCors("Frontend");
app.MapControllers();
app.Run();