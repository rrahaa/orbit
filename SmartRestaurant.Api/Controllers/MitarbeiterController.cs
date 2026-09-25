using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Api.Data;
using SmartRestaurant.Shared.Dtos;

namespace SmartRestaurant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MitarbeiterController : ControllerBase
{
    private readonly AppDbContext _db;
    public MitarbeiterController(AppDbContext db) => _db = db;

    // GET /api/mitarbeiter
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var liste = await _db.Mitarbeiter
            .Include(m => m.Rolle)
            .Select(m => new MitarbeiterDto
            {
                MitarbeiterId = (int)m.MitarbeiterId,
                Name = m.Name,
                Benutzername = m.Benutzername,
                Rolle = m.Rolle.Rollenname,
                IstAktiv = m.IstAktiv ?? true
            })
            .ToListAsync();

        return Ok(liste);
    }

    // POST /api/mitarbeiter
    [HttpPost]
    public async Task<IActionResult> Create(NeuerMitarbeiterDto dto)
    {
        var rolle = await _db.Rolle.FindAsync((uint)dto.RolleId);
        if (rolle is null) return BadRequest("Rolle existiert nicht.");

        var mitarbeiter = new Models.Mitarbeiter
        {
            Name = dto.Name,
            Benutzername = dto.Benutzername,
            PasswortHash = BCrypt.Net.BCrypt.HashPassword(dto.Passwort),
            RolleId = (uint)dto.RolleId,
            IstAktiv = true
        };

        _db.Mitarbeiter.Add(mitarbeiter);
        await _db.SaveChangesAsync();

        return Ok(new MitarbeiterDto
        {
            MitarbeiterId = (int)mitarbeiter.MitarbeiterId,
            Name = mitarbeiter.Name,
            Benutzername = mitarbeiter.Benutzername,
            Rolle = rolle.Rollenname,
            IstAktiv = mitarbeiter.IstAktiv ?? true
        });
    }

    // PUT /api/mitarbeiter/3
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(uint id, NeuerMitarbeiterDto dto)
    {
        var mitarbeiter = await _db.Mitarbeiter.FindAsync(id);
        if (mitarbeiter is null) return NotFound();

        mitarbeiter.Name = dto.Name;
        mitarbeiter.Benutzername = dto.Benutzername;
        mitarbeiter.RolleId = (uint)dto.RolleId;

        if (!string.IsNullOrWhiteSpace(dto.Passwort))
            mitarbeiter.PasswortHash = BCrypt.Net.BCrypt.HashPassword(dto.Passwort);

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/mitarbeiter/3
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(uint id)
    {
        var mitarbeiter = await _db.Mitarbeiter.FindAsync(id);
        if (mitarbeiter is null) return NotFound();

        mitarbeiter.IstAktiv = false; // löschen = deaktivieren, wegen Bestellungen-Verknüpfung
        await _db.SaveChangesAsync();
        return NoContent();
    }
}