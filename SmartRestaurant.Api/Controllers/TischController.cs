using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Api.Data;

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
}