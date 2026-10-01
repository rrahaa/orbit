using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Api.Data;

namespace SmartRestaurant.Api.Controllers;

[ApiController]
[Route("api")]
public class StammdatenController : ControllerBase
{
    private readonly AppDbContext _db;
    public StammdatenController(AppDbContext db) => _db = db;

    [HttpGet("rollen")]
    public async Task<IActionResult> GetRollen() =>
        Ok(await _db.Rolle
            .Select(r => new { rolleId = (int)r.RolleId, name = r.Rollenname })
            .ToListAsync());

    [HttpGet("kategorien")]
    public async Task<IActionResult> GetKategorien() =>
        Ok(await _db.ArtikelKategorie
            .Select(k => new { kategorieId = (int)k.KategorieId, name = k.Kategoriename })
            .ToListAsync());
}