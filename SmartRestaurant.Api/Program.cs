using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Api.Data;

var builder = WebApplication.CreateBuilder(args);

var cs = builder.Configuration.GetConnectionString("Default")!;
builder.Services.AddDbContext<AppDbContext>(o => o.UseMySql(cs, ServerVersion.AutoDetect(cs)));
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();
app.Run();