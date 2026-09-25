using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Api.Data;
using SmartRestaurant.Shared.Dtos;

namespace SmartRestaurant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BarController : ControllerBase
{
    private readonly AppDbContext _db;
    public BarController(AppDbContext db) => _db = db;

    // GET /api/bar/offen -> offene und in Bearbeitung befindliche Bestellungen, nach Zeit sortiert
    [HttpGet("offen")]
    public async Task<IActionResult> GetOffen()
    {
        var bestellungen = await _db.Bestellung
            .Where(b => b.BestellStatusId == 1 || b.BestellStatusId == 2) // Offen, In Bearbeitung
            .OrderBy(b => b.Bestelldatum)
            .Include(b => b.BestellStatus)
            .Include(b => b.Mitarbeiter)
            .Include(b => b.Tisch)
            .Include(b => b.Bestellposition).ThenInclude(p => p.Artikel)
            .ToListAsync();

        var result = bestellungen.Select(b => new BestellungDto
        {
            BestellungId = (int)b.BestellungId,
            TischId = (int)b.TischId,
            Tischnummer = (int)b.Tisch.Tischnummer,
            MitarbeiterName = b.Mitarbeiter.Name,
            Status = b.BestellStatus.Statusname,
            Bestelldatum = b.Bestelldatum,
            Abschlusszeitpunkt = b.Abschlusszeitpunkt,
            Positionen = b.Bestellposition.Select(p => new BestellPositionDto
            {
                BestellpositionId = (int)p.BestellpositionId,
                ArtikelId = (int)p.ArtikelId,
                ArtikelName = p.Artikel.Name,
                Menge = (int)p.Menge,
                Einzelpreis = p.Einzelpreis
            }).ToList()
        });

        return Ok(result);
    }
}