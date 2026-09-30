
using Microsoft.AspNetCore.Mvc;
using DbModels;
using Services;

namespace AppWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttractionController : ControllerBase
{
    private readonly ILogger<AttractionController> _logger;
    private readonly IAttractionService _service;

    public AttractionController(ILogger<AttractionController> logger, IAttractionService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("ReadAll")]
    public async Task<ActionResult<List<AttractionDbM>>> ReadAll()
    {
        var result = await _service.ReadAllAttractionsAsync();
        return Ok(result);
    }
}



