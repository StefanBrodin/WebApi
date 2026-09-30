using Microsoft.AspNetCore.Mvc;
using DbModels;
using Services;

namespace AppWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ILogger<CustomerController> _logger;
    private readonly ICustomerService _service;

    public CustomerController(ILogger<CustomerController> logger, ICustomerService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("ReadAll")]
    public async Task<ActionResult<List<CustomerDbM>>> ReadAll()
    {
        var result = await _service.ReadAllCustomersAsync();
        return Ok(result);
    }
}




