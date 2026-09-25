using Microsoft.AspNetCore.Mvc;
using SmartRestaurant.Api.Data;
using SmartRestaurant.Shared.Dtos;

namespace SmartRestaurant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TischeAdminController : ControllerBase
{
    private readonly AppDbContext _db;
    public TischeAdminController(AppDbContext db) => _db = db;

    [HttpPost]
    public async Task<IActionResult> Create(NeuerTischDto dto)
    {
        var tisch = new Models.Tisch
        {
            Tischnummer = (uint)dto.Tischnummer,
            Kapazitaet = (uint)dto.Kapazitaet,
            TischStatusId = 1 // frei
        };

        _db.Tisch.Add(tisch);
        await _db.SaveChangesAsync();
        return Ok(new { tisch.TischId, tisch.Tischnummer, tisch.Kapazitaet });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(uint id, NeuerTischDto dto)
    {
        var tisch = await _db.Tisch.FindAsync(id);
        if (tisch is null) return NotFound();

        tisch.Tischnummer = (uint)dto.Tischnummer;
        tisch.Kapazitaet = (uint)dto.Kapazitaet;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(uint id)
    {
        var tisch = await _db.Tisch.FindAsync(id);
        if (tisch is null) return NotFound();

        _db.Tisch.Remove(tisch);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}