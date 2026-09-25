using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Api.Data;
using SmartRestaurant.Shared.Dtos;

namespace SmartRestaurant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatistikController : ControllerBase
{
    private readonly AppDbContext _db;
    public StatistikController(AppDbContext db) => _db = db;

    // GET /api/statistik/woche  -> Umsatz und meistverkauftes Getränk der letzten 7 Tage
    [HttpGet("woche")]
    public async Task<IActionResult> GetWoche()
    {
        var seit = DateTime.Now.AddDays(-7);

        var positionenDieserWoche = await _db.Bestellposition
            .Include(p => p.Bestellung)
            .Include(p => p.Artikel)
            .Where(p => p.Bestellung.Bestelldatum >= seit && p.Bestellung.BestellStatusId == 5) // nur bezahlte
            .ToListAsync();

        var umsatz = positionenDieserWoche.Sum(p => p.Menge * p.Einzelpreis);

        var meistverkauft = positionenDieserWoche
            .GroupBy(p => p.Artikel.Name)
            .Select(g => new { Name = g.Key, Menge = g.Sum(p => (int)p.Menge) })
            .OrderByDescending(g => g.Menge)
            .FirstOrDefault();

        return Ok(new StatistikDto
        {
            Wochenumsatz = umsatz,
            MeistverkauftesGetraenk = meistverkauft?.Name,
            MeistverkaufteMenge = meistverkauft?.Menge ?? 0
        });
    }
}