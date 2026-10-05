using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Api.Data;
using SmartRestaurant.Api.Models;
using SmartRestaurant.Shared.Dtos;

namespace SmartRestaurant.Api.Controllers;

[ApiController]
[Route("api/kategorien")]
public class KategorienController : ControllerBase
{
    private readonly AppDbContext _db;
    
    public KategorienController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var kategorien =
            await _db.ArtikelKategorie
                .OrderBy(k => k.Kategoriename)
                .Select(k => new
                {
                    kategorieId = (int)k.KategorieId,
                    name = k.Kategoriename
                })
                .ToListAsync();

        return Ok(kategorien);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        NeueKategorieDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest(
                "Der Kategoriename darf nicht leer sein.");
        }

        var name = dto.Name.Trim();

        var existiertBereits =
            await _db.ArtikelKategorie
                .AnyAsync(k =>
                    k.Kategoriename.ToLower() ==
                    name.ToLower());

        if (existiertBereits)
        {
            return Conflict(
                "Diese Kategorie existiert bereits.");
        }

        var kategorie = new ArtikelKategorie
        {
            Kategoriename = name
        };

        _db.ArtikelKategorie.Add(kategorie);

        await _db.SaveChangesAsync();

        return Ok(new
        {
            kategorieId = (int)kategorie.KategorieId,
            name = kategorie.Kategoriename
        });
    }
    

}