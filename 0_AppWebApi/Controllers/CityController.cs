using Microsoft.AspNetCore.Mvc;
using DbModels;
using Services;

namespace AppWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CityController : ControllerBase
{
    private readonly ILogger<CityController> _logger;
    private readonly ICityService _service;

    public CityController(ILogger<CityController> logger, ICityService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("ReadAll")]
    public async Task<ActionResult<List<CityDbM>>> ReadAll()
    {
        var result = await _service.ReadAllCitiesAsync();
        return Ok(result);
    }
}