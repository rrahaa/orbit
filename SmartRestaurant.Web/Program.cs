using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SmartRestaurant.Web;
using SmartRestaurant.Web.Auth;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// SmartRestaurant.Api HTTP profile
builder.Services.AddScoped(_ => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5274/")
});

// Anmeldung / Rollen
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<AnmeldeStatusProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(
    sp => sp.GetRequiredService<AnmeldeStatusProvider>());

builder.Services.AddMudServices();

await builder.Build().RunAsync();