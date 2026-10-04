using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Api.Data;
using SmartRestaurant.Shared.Dtos;

namespace SmartRestaurant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly AppDbContext _db;

    public LoginController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var mitarbeiter = await _db.Mitarbeiter
            .Include(m => m.Rolle)
            .FirstOrDefaultAsync(m => m.Benutzername == dto.Benutzername);

        if (mitarbeiter == null)
        {
            return Unauthorized("Benutzername oder Passwort ist falsch.");
        }

        if (mitarbeiter.IstAktiv == false)
        {
            return Unauthorized("Dieser Benutzer ist deaktiviert.");
        }

        bool passwortRichtig;
        try
        {
            passwortRichtig = BCrypt.Net.BCrypt.Verify(dto.Passwort, mitarbeiter.PasswortHash);
        }
        catch (BCrypt.Net.SaltParseException)
        {
            // Kein gültiger BCrypt-Hash in der Datenbank -> Login ablehnen statt 500
            passwortRichtig = false;
        }

        if (!passwortRichtig)
        {
            return Unauthorized("Benutzername oder Passwort ist falsch.");
        }

        var antwort = new LoginAntwortDto
        {
            MitarbeiterId = (int)mitarbeiter.MitarbeiterId,
            Name = mitarbeiter.Name,
            Rolle = mitarbeiter.Rolle.Rollenname
        };

        return Ok(antwort);
    }
}
