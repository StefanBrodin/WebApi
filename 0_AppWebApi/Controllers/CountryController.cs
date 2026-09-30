using Microsoft.AspNetCore.Mvc;
using DbModels;
using Services;

namespace AppWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountryController : ControllerBase
{
    private readonly ILogger<CountryController> _logger;
    private readonly ICountryService _service;

    public CountryController(ILogger<CountryController> logger, ICountryService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("ReadAll")]
    public async Task<ActionResult<List<CountryDbM>>> ReadAll()
    {
        var result = await _service.ReadAllCountriesAsync();
        return Ok(result);
    }
}