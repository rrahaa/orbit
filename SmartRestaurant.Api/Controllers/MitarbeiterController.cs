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

    public MitarbeiterController(AppDbContext db)
    {
        _db = db;
    }

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

    [HttpPost]
    public async Task<IActionResult> Create(NeuerMitarbeiterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest("Der Name darf nicht leer sein.");

        if (string.IsNullOrWhiteSpace(dto.Benutzername))
            return BadRequest("Der Benutzername darf nicht leer sein.");

        if (string.IsNullOrWhiteSpace(dto.Passwort))
            return BadRequest("Das Passwort darf nicht leer sein.");

        if (dto.RolleId <= 0)
            return BadRequest("Bitte wählen Sie eine Rolle aus.");

        var rolle = await _db.Rolle.FindAsync((uint)dto.RolleId);

        if (rolle is null)
            return BadRequest("Die ausgewählte Rolle existiert nicht.");

        var benutzername = dto.Benutzername.Trim();

        var benutzernameExistiert = await _db.Mitarbeiter
            .AnyAsync(m => m.Benutzername == benutzername);

        if (benutzernameExistiert)
        {
            return Conflict(
                $"Der Benutzername „{benutzername}“ ist bereits vergeben. Bitte wählen Sie einen anderen Benutzernamen.");
        }

        var mitarbeiter = new Models.Mitarbeiter
        {
            Name = dto.Name.Trim(),
            Benutzername = benutzername,
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(uint id, NeuerMitarbeiterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest("Der Name darf nicht leer sein.");

        if (string.IsNullOrWhiteSpace(dto.Benutzername))
            return BadRequest("Der Benutzername darf nicht leer sein.");

        if (dto.RolleId <= 0)
            return BadRequest("Bitte wählen Sie eine Rolle aus.");

        var mitarbeiter = await _db.Mitarbeiter.FindAsync(id);

        if (mitarbeiter is null)
            return NotFound("Der Mitarbeiter wurde nicht gefunden.");

        var rolle = await _db.Rolle.FindAsync((uint)dto.RolleId);

        if (rolle is null)
            return BadRequest("Die ausgewählte Rolle existiert nicht.");

        var benutzername = dto.Benutzername.Trim();

        var benutzernameExistiert = await _db.Mitarbeiter
            .AnyAsync(m =>
                m.Benutzername == benutzername &&
                m.MitarbeiterId != id);

        if (benutzernameExistiert)
        {
            return Conflict(
                $"Der Benutzername „{benutzername}“ ist bereits vergeben. Bitte wählen Sie einen anderen Benutzernamen.");
        }

        mitarbeiter.Name = dto.Name.Trim();
        mitarbeiter.Benutzername = benutzername;
        mitarbeiter.RolleId = (uint)dto.RolleId;

        if (!string.IsNullOrWhiteSpace(dto.Passwort))
        {
            mitarbeiter.PasswortHash =
                BCrypt.Net.BCrypt.HashPassword(dto.Passwort);
        }

        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(uint id)
    {
        var mitarbeiter = await _db.Mitarbeiter.FindAsync(id);

        if (mitarbeiter is null)
            return NotFound("Der Mitarbeiter wurde nicht gefunden.");

        mitarbeiter.IstAktiv = false;

        await _db.SaveChangesAsync();

        return NoContent();
    }
}