using Microsoft.AspNetCore.Mvc;
using DbModels;
using Services;

namespace AppWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressController : ControllerBase
{
    private readonly ILogger<AddressController> _logger;
    private readonly IAddressService _service;

    public AddressController(ILogger<AddressController> logger, IAddressService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("ReadAll")]
    public async Task<ActionResult<List<AddressDbM>>> ReadAll()
    {
        var result = await _service.ReadAllAddressesAsync();
        return Ok(result);
    }
}