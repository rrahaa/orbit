using System.Net;
using System.Net.Http.Json;
using SmartRestaurant.Shared.Dtos;

namespace SmartRestaurant.Web.Shared;

/// <summary>Virtueller Buchungstisch „Bar“ ohne Änderungen an API oder DB-Schema.</summary>
public static class BarBookingTable
{
    // Reservierte Nummer, da die bestehende Tisch-Tabelle kein Namensfeld besitzt.
    public const int Number = 1000000;

    public static async Task<int> EnsureAsync(HttpClient http, List<TischUebersichtDto> tables)
    {
        var existing = tables.FirstOrDefault(t => t.Tischnummer == Number);
        if (existing is not null) return existing.TischId;

        using var response = await http.PostAsJsonAsync("api/tische", new NeuerTischDto
        {
            Tischnummer = Number, Kapazitaet = 2
        });
        // Gleichzeitiges Öffnen der Bar auf mehreren Geräten darf denselben Tisch verwenden.
        if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.Conflict)
            throw new InvalidOperationException($"Bar konnte nicht eingerichtet werden ({(int)response.StatusCode}). {await response.Content.ReadAsStringAsync()}");

        var updated = await http.GetFromJsonAsync<List<TischUebersichtDto>>("api/tische") ?? new();
        return updated.FirstOrDefault(t => t.Tischnummer == Number)?.TischId
            ?? throw new InvalidOperationException("Der virtuelle Buchungstisch Bar wurde nicht gefunden.");
    }
}
