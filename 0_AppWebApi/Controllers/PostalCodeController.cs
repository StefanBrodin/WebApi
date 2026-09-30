using Microsoft.AspNetCore.Mvc;
using DbModels;
using Services;

namespace AppWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostalCodeController : ControllerBase
{
    private readonly ILogger<PostalCodeController> _logger;
    private readonly IPostalCodeService _service;

    public PostalCodeController(ILogger<PostalCodeController> logger, IPostalCodeService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("ReadAll")]
    public async Task<ActionResult<List<PostalCodeDbM>>> ReadAll()
    {
        var result = await _service.ReadAllPostalCodesAsync();
        return Ok(result);
    }
}