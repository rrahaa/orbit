using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Api.Data;
using SmartRestaurant.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var cs = builder.Configuration.GetConnectionString("Default")!;
builder.Services.AddDbContext<AppDbContext>(o => o.UseMySql(cs, ServerVersion.AutoDetect(cs)));
builder.Services.AddControllers();
builder.Services.AddSingleton<StatusLogService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins("https://localhost:7102", "http://localhost:5048")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("Frontend");
app.MapControllers();
app.Run();