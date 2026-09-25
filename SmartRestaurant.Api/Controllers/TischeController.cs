using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Api.Data;
using SmartRestaurant.Shared.Dtos;

namespace SmartRestaurant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TischeController : ControllerBase
{
    private readonly AppDbContext _db;
    public TischeController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _db.Tisch
            .Select(t => new { t.TischId, t.Tischnummer, t.Kapazitaet, t.TischStatusId })
            .ToListAsync());
    
    [HttpGet("{id}/rechnung")]
    public async Task<IActionResult> GetRechnung(uint id)
    {
        var tisch = await _db.Tisch.FindAsync(id);
        if (tisch is null) return NotFound();

        var bestellungen = await _db.Bestellung
            .Where(b => b.TischId == id && b.BestellStatusId != 5) // alles außer "Bezahlt"
            .Include(b => b.BestellStatus)
            .Include(b => b.Mitarbeiter)
            .Include(b => b.Tisch)
            .Include(b => b.Bestellposition).ThenInclude(p => p.Artikel)
            .ToListAsync();

        var rechnung = new RechnungDto
        {
            TischId = (int)tisch.TischId,
            Tischnummer = (int)tisch.Tischnummer,
            Bestellungen = bestellungen.Select(b => new BestellungDto
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
            }).ToList()
        };

        return Ok(rechnung);
    }
}