using Microsoft.AspNetCore.Mvc;
using SmartRestaurant.Api.Services;

namespace SmartRestaurant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatuslogController : ControllerBase
{
    private readonly StatusLogService _statusLog;
    public StatuslogController(StatusLogService statusLog) => _statusLog = statusLog;

    [HttpGet]
    public IActionResult GetAll() => Ok(_statusLog.AlleLesen());
}