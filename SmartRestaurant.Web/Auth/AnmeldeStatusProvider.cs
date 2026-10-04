using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using SmartRestaurant.Shared.Dtos;

namespace SmartRestaurant.Web.Auth;

// Merkt sich den angemeldeten Mitarbeiter im localStorage und stellt ihn
// Blazor als AuthenticationState (inkl. Rolle) zur Verfügung.
public class AnmeldeStatusProvider : AuthenticationStateProvider
{
    private const string SpeicherSchluessel = "smartgastro.benutzer";

    private static readonly AuthenticationState Anonym =
        new(new ClaimsPrincipal(new ClaimsIdentity()));

    private readonly IJSRuntime _js;
    private AuthenticationState? _aktuell;

    public AnmeldeStatusProvider(IJSRuntime js)
    {
        _js = js;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_aktuell != null)
        {
            return _aktuell;
        }

        try
        {
            var json = await _js.InvokeAsync<string?>("localStorage.getItem", SpeicherSchluessel);
            var benutzer = string.IsNullOrEmpty(json)
                ? null
                : JsonSerializer.Deserialize<LoginAntwortDto>(json);

            _aktuell = benutzer == null ? Anonym : ErzeugeStatus(benutzer);
        }
        catch (JsonException)
        {
            _aktuell = Anonym;
        }

        return _aktuell;
    }

    public async Task AnmeldenAsync(LoginAntwortDto benutzer)
    {
        await _js.InvokeVoidAsync("localStorage.setItem",
            SpeicherSchluessel, JsonSerializer.Serialize(benutzer));

        _aktuell = ErzeugeStatus(benutzer);
        NotifyAuthenticationStateChanged(Task.FromResult(_aktuell));
    }

    public async Task AbmeldenAsync()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", SpeicherSchluessel);

        _aktuell = Anonym;
        NotifyAuthenticationStateChanged(Task.FromResult(_aktuell));
    }

    private static AuthenticationState ErzeugeStatus(LoginAntwortDto benutzer)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, benutzer.MitarbeiterId.ToString()),
            new(ClaimTypes.Name, benutzer.Name),
            new(ClaimTypes.Role, benutzer.Rolle)
        };

        var identity = new ClaimsIdentity(claims, "SmartGastro");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }
}
