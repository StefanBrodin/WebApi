using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
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

    // GET: /api/Customer/Read?seeded=true&flat=false&filter=&pageNumber=0&pageSize=10
    [HttpGet("Read")]
    public async Task<ActionResult<ResponsePageDto<ICustomer>>> Read(
        [FromQuery] bool seeded = true,
        [FromQuery] bool flat = false,
        [FromQuery] string filter = null,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var result = await _service.ReadCustomersAsync(seeded, flat, filter, pageNumber, pageSize);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading customers page");
            return BadRequest(ex.Message);
        }
    }

    // GET: /api/Customer/ReadWithReviews?seeded=true&filter=&pageNumber=0&pageSize=10
    // Explicitly fulfils the requirement to read customers with reviews, by setting flat=false
    [HttpGet("ReadWithReviews")]
    public async Task<ActionResult<ResponsePageDto<ICustomer>>> ReadWithReviews(
        [FromQuery] bool seeded = true,
        [FromQuery] string filter = null,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var result = await _service.ReadCustomersAsync(seeded, false, filter, pageNumber, pageSize);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading customers with reviews");
            return BadRequest(ex.Message);
        }
    }

    // GET: /api/Customer/ReadItem?id=...&flat=false
    [HttpGet("ReadItem")]
    public async Task<ActionResult<ResponseItemDto<ICustomer>>> ReadItem(
        [FromQuery] Guid id,
        [FromQuery] bool flat = false)
    {
        try
        {
            var result = await _service.ReadCustomerAsync(id, flat);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading customer item");
            return BadRequest(ex.Message);
        }
    }
}