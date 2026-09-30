using Microsoft.AspNetCore.Mvc;
using DbModels;
using Services;

namespace AppWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerAttractionRatingController : ControllerBase
{
    private readonly ILogger<CustomerAttractionRatingController> _logger;
    private readonly ICustomerAttractionRatingService _service;

    public CustomerAttractionRatingController(ILogger<CustomerAttractionRatingController> logger, ICustomerAttractionRatingService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("ReadAll")]
    public async Task<ActionResult<List<CustomerAttractionRatingDbM>>> ReadAll()
    {
        var result = await _service.ReadAllRatingsAsync();
        return Ok(result);
    }
}


