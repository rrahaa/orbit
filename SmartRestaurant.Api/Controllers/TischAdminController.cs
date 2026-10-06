using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Api.Data;
using SmartRestaurant.Shared.Dtos;

namespace SmartRestaurant.Api.Controllers;

[ApiController]
[Route("api/tische")]
public class TischeAdminController : ControllerBase
{
    private readonly AppDbContext _db;

    public TischeAdminController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> Create(NeuerTischDto dto)
    {
        if (dto.Tischnummer <= 0)
            return BadRequest("Die Tischnummer muss größer als 0 sein.");

        if (dto.Kapazitaet < 2 || dto.Kapazitaet > 8)
            return BadRequest("Die Kapazität muss zwischen 2 und 8 Personen liegen.");

        var existiert = await _db.Tisch
            .AnyAsync(t => t.Tischnummer == (uint)dto.Tischnummer);

        if (existiert)
            return Conflict($"Tisch {dto.Tischnummer} existiert bereits. Bitte wählen Sie eine andere Tischnummer.");

        var tisch = new Models.Tisch
        {
            Tischnummer = (uint)dto.Tischnummer,
            Kapazitaet = (uint)dto.Kapazitaet,
            TischStatusId = 1
        };

        _db.Tisch.Add(tisch);
        await _db.SaveChangesAsync();

        return Ok(new
        {
            tisch.TischId,
            tisch.Tischnummer,
            tisch.Kapazitaet
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(uint id, NeuerTischDto dto)
    {
        if (dto.Tischnummer <= 0)
            return BadRequest("Die Tischnummer muss größer als 0 sein.");

        if (dto.Kapazitaet < 2 || dto.Kapazitaet > 8)
            return BadRequest("Die Kapazität muss zwischen 2 und 8 Personen liegen.");

        var tisch = await _db.Tisch.FindAsync(id);

        if (tisch is null)
            return NotFound("Der Tisch wurde nicht gefunden.");

        var nummerBereitsVergeben = await _db.Tisch
            .AnyAsync(t =>
                t.Tischnummer == (uint)dto.Tischnummer &&
                t.TischId != id);

        if (nummerBereitsVergeben)
            return Conflict($"Tisch {dto.Tischnummer} existiert bereits. Bitte wählen Sie eine andere Tischnummer.");

        tisch.Tischnummer = (uint)dto.Tischnummer;
        tisch.Kapazitaet = (uint)dto.Kapazitaet;

        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(uint id)
    {
        var tisch = await _db.Tisch.FindAsync(id);

        if (tisch is null)
            return NotFound("Der Tisch wurde nicht gefunden.");

        _db.Tisch.Remove(tisch);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}