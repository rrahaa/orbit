using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Api.Data;
using SmartRestaurant.Shared.Dtos;

namespace SmartRestaurant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BestellungenController : ControllerBase
{
    private readonly AppDbContext _db;
    public BestellungenController(AppDbContext db) => _db = db;

    // GET /api/bestellungen/tisch/3  -> alle Bestellungen eines Tisches
    [HttpGet("tisch/{tischId}")]
    public async Task<IActionResult> GetByTisch(uint tischId)
    {
        var bestellungen = await _db.Bestellung
            .Where(b => b.TischId == tischId)
            .Include(b => b.BestellStatus)
            .Include(b => b.Mitarbeiter)
            .Include(b => b.Tisch)
            .Include(b => b.Bestellposition).ThenInclude(p => p.Artikel)
            .ToListAsync();

        return Ok(bestellungen.Select(ToDto));
    }

    // POST /api/bestellungen  -> neue Bestellung mit Positionen anlegen
    [HttpPost]
    public async Task<IActionResult> Create(NeueBestellungDto dto)
    {
        if (dto.Positionen.Count == 0)
            return BadRequest("Eine Bestellung braucht mindestens eine Position.");

        var artikelIds = dto.Positionen.Select(p => (uint)p.ArtikelId).ToList();
        var artikel = await _db.Artikel
            .Where(a => artikelIds.Contains(a.ArtikelId))
            .ToDictionaryAsync(a => a.ArtikelId);

        var bestellung = new Models.Bestellung
        {
            TischId = (uint)dto.TischId,
            MitarbeiterId = (uint)dto.MitarbeiterId,
            BestellStatusId = 1, // 1 = Offen
            Bestelldatum = DateTime.Now
        };

        foreach (var pos in dto.Positionen)
        {
            if (!artikel.TryGetValue((uint)pos.ArtikelId, out var a))
                return BadRequest($"Artikel {pos.ArtikelId} existiert nicht.");

            bestellung.Bestellposition.Add(new Models.Bestellposition
            {
                ArtikelId = a.ArtikelId,
                Menge = (uint)pos.Menge,
                Einzelpreis = a.Preis
            });
        }

        _db.Bestellung.Add(bestellung);
        await _db.SaveChangesAsync();

        // Tisch auf "besetzt" setzen
        var tisch = await _db.Tisch.FindAsync(bestellung.TischId);
        if (tisch is not null) tisch.TischStatusId = 2; // 2 = besetzt
        await _db.SaveChangesAsync();

        await _db.Entry(bestellung).Reference(b => b.BestellStatus).LoadAsync();
        await _db.Entry(bestellung).Reference(b => b.Mitarbeiter).LoadAsync();
        await _db.Entry(bestellung).Reference(b => b.Tisch).LoadAsync();
        await _db.Entry(bestellung).Collection(b => b.Bestellposition)
            .Query().Include(p => p.Artikel).LoadAsync();

        return CreatedAtAction(nameof(GetByTisch), new { tischId = bestellung.TischId }, ToDto(bestellung));
    }

    private static BestellungDto ToDto(Models.Bestellung b) => new()
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
    };
    
    // PUT /api/bestellungen/5/status
    [HttpPut("{id}/status")]
    public async Task<IActionResult> SetStatus(uint id, StatusAendernDto dto)
    {
        var bestellung = await _db.Bestellung.FindAsync(id);
        if (bestellung is null) return NotFound();

        bestellung.BestellStatusId = (uint)dto.NeuerStatusId;

        if (dto.NeuerStatusId == 5) // Bezahlt
        {
            bestellung.Abschlusszeitpunkt = DateTime.Now;
            await _db.SaveChangesAsync();

            // Tisch nur freigeben, wenn KEINE andere Bestellung dieses Tisches noch offen ist
            var nochOffen = await _db.Bestellung
                .AnyAsync(b => b.TischId == bestellung.TischId && b.BestellStatusId != 5);

            if (!nochOffen)
            {
                var tisch = await _db.Tisch.FindAsync(bestellung.TischId);
                if (tisch is not null) tisch.TischStatusId = 1; // frei
            }
        }

        await _db.SaveChangesAsync();
        return NoContent();
    }
}