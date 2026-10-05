using System.Text.Json;
using SmartRestaurant.Shared.Dtos;

namespace SmartRestaurant.Api.Services;

public class StatusLogService
{
    private readonly string _pfad;
    private static readonly object _schloss = new();

    public StatusLogService(IWebHostEnvironment env)
    {
        var ordner = Path.Combine(env.ContentRootPath, "StatusLogs");
        Directory.CreateDirectory(ordner);
        _pfad = Path.Combine(ordner, "status-log.jsonl");
    }

    public void Eintragen(StatusLogEintragDto eintrag)
    {
        var zeile = JsonSerializer.Serialize(eintrag) + Environment.NewLine;
        lock (_schloss)
        {
            File.AppendAllText(_pfad, zeile);
        }
    }

    public List<StatusLogEintragDto> AlleLesen()
    {
        lock (_schloss)
        {
            if (!File.Exists(_pfad)) return new();

            return File.ReadAllLines(_pfad)
                .Where(z => !string.IsNullOrWhiteSpace(z))
                .Select(z => JsonSerializer.Deserialize<StatusLogEintragDto>(z)!)
                .ToList();
        }
    }
}