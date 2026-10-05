
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Api.Data;

namespace SmartRestaurant.Api.Controllers;

[ApiController]
[Route("api")]
public class StammdatenController : ControllerBase
{
    private readonly AppDbContext _db;

    public StammdatenController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("rollen")]
    public async Task<IActionResult> GetRollen()
    {
        var rollen = await _db.Rolle
            .OrderBy(r => r.Rollenname)
            .Select(r => new
            {
                rolleId = (int)r.RolleId,
                name = r.Rollenname
            })
            .ToListAsync();

        return Ok(rollen);
    }
}
