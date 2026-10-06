using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Api.Data;
using SmartRestaurant.Shared.Dtos;

namespace SmartRestaurant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtikelController : ControllerBase
{
private readonly AppDbContext _db;

public ArtikelController(AppDbContext db)
{
    _db = db;
}

[HttpGet]
public async Task<IActionResult> GetAll()
{
    var liste = await _db.Artikel
        .Include(a => a.Kategorie)
        .Select(a => new ArtikelDto
        {
            ArtikelId = (int)a.ArtikelId,
            Name = a.Name,
            Preis = a.Preis,
            KategorieId = (int)a.KategorieId,
            Kategorie = a.Kategorie.Kategoriename
        })
        .ToListAsync();

    return Ok(liste);
}

[HttpPost]
public async Task<IActionResult> Create(NeuerArtikelDto dto)
{
    if (string.IsNullOrWhiteSpace(dto.Name))
        return BadRequest("Der Artikelname darf nicht leer sein.");

    if (dto.KategorieId <= 0)
        return BadRequest("Bitte eine Kategorie auswählen.");

    if (dto.Preis < 0)
        return BadRequest("Der Preis darf nicht negativ sein.");

    var name = dto.Name.Trim();

    var kategorie =
        await _db.ArtikelKategorie
            .FindAsync((uint)dto.KategorieId);

    if (kategorie is null)
        return BadRequest("Die ausgewählte Kategorie existiert nicht.");

    var existiertBereits =
        await _db.Artikel.AnyAsync(a =>
            a.KategorieId == (uint)dto.KategorieId &&
            a.Name.ToLower() == name.ToLower());

    if (existiertBereits)
    {
        return Conflict(
            "Ein Artikel mit diesem Namen existiert bereits in dieser Kategorie.");
    }

    var artikel = new Models.Artikel
    {
        Name = name,
        Preis = dto.Preis,
        KategorieId = (uint)dto.KategorieId
    };

    _db.Artikel.Add(artikel);

    await _db.SaveChangesAsync();

    return Ok(new ArtikelDto
    {
        ArtikelId = (int)artikel.ArtikelId,
        Name = artikel.Name,
        Preis = artikel.Preis,
        KategorieId = (int)artikel.KategorieId,
        Kategorie = kategorie.Kategoriename
    });
}

[HttpPut("{id}")]
public async Task<IActionResult> Update(
    uint id,
    NeuerArtikelDto dto)
{
    if (string.IsNullOrWhiteSpace(dto.Name))
        return BadRequest("Der Artikelname darf nicht leer sein.");

    if (dto.KategorieId <= 0)
        return BadRequest("Bitte eine Kategorie auswählen.");

    if (dto.Preis < 0)
        return BadRequest("Der Preis darf nicht negativ sein.");

    var artikel =
        await _db.Artikel.FindAsync(id);

    if (artikel is null)
        return NotFound("Artikel wurde nicht gefunden.");

    var kategorie =
        await _db.ArtikelKategorie
            .FindAsync((uint)dto.KategorieId);

    if (kategorie is null)
        return BadRequest("Die ausgewählte Kategorie existiert nicht.");

    var name = dto.Name.Trim();

    var existiertBereits =
        await _db.Artikel.AnyAsync(a =>
            a.ArtikelId != id &&
            a.KategorieId == (uint)dto.KategorieId &&
            a.Name.ToLower() == name.ToLower());

    if (existiertBereits)
    {
        return Conflict(
            "Ein Artikel mit diesem Namen existiert bereits in dieser Kategorie.");
    }

    artikel.Name = name;
    artikel.Preis = dto.Preis;
    artikel.KategorieId = (uint)dto.KategorieId;

    await _db.SaveChangesAsync();

    return NoContent();
}

[HttpDelete("{id}")]
public async Task<IActionResult> Delete(uint id)
{
    var artikel =
        await _db.Artikel.FindAsync(id);

    if (artikel is null)
        return NotFound("Artikel wurde nicht gefunden.");

    var wurdeSchonBestellt =
        await _db.Bestellposition
            .AnyAsync(p => p.ArtikelId == id);

    if (wurdeSchonBestellt)
    {
        return BadRequest(
            "Artikel wurde schon bestellt und kann nicht gelöscht werden.");
    }

    _db.Artikel.Remove(artikel);

    await _db.SaveChangesAsync();

    return NoContent();
}


}
