using Microsoft.AspNetCore.Mvc;
using DbModels;
using Services;
using Models;
using Models.DTO;


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

    // GET: /api/CustomerAttractionRating/ReadItem?customerId=...&attractionId=...
    [HttpGet("ReadItem")]
    [ProducesResponseType(200, Type = typeof(ResponseItemDto<ICustomerAttractionRating>))]
    [ProducesResponseType(400, Type = typeof(string))]
    [ProducesResponseType(404, Type = typeof(string))]
    public async Task<ActionResult<ResponseItemDto<ICustomerAttractionRating>>> ReadItem(
        [FromQuery] Guid customerId,
        [FromQuery] Guid attractionId)
    {
        try
        {
            var result = await _service.ReadRatingAsync(customerId, attractionId);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading rating item");
            return BadRequest(ex.Message);
        }
    }

    // POST: /api/CustomerAttractionRating/CreateItem
    [HttpPost("CreateItem")]
    [ProducesResponseType(200, Type = typeof(ResponseItemDto<ICustomerAttractionRating>))]
    [ProducesResponseType(400, Type = typeof(string))]
    public async Task<IActionResult> CreateItem([FromBody] CustomerAttractionRatingCuDto item)
    {
        try
        {
            item.EnsureValidity();
            _logger.LogInformation($"{nameof(CreateItem)}: Creating rating for customer {item.CustomerId} and attraction {item.AttractionId}");

            var result = await _service.CreateRatingAsync(item);
            _logger.LogInformation($"Rating created successfully");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not create rating");
            return BadRequest($"Could not create. Error: {ex.Message}");
        }
    }

    // DELETE: /api/CustomerAttractionRating/DeleteItem?customerId=...&attractionId=...
    [HttpDelete("DeleteItem")]
    [ProducesResponseType(200, Type = typeof(ResponseItemDto<ICustomerAttractionRating>))]
    [ProducesResponseType(400, Type = typeof(string))]
    [ProducesResponseType(404, Type = typeof(string))]
    public async Task<IActionResult> DeleteItem([FromQuery] Guid customerId, [FromQuery] Guid attractionId)
    {
        try
        {
            _logger.LogInformation($"{nameof(DeleteItem)}: Deleting rating for customer {customerId} and attraction {attractionId}");

            var result = await _service.DeleteRatingAsync(customerId, attractionId);
            _logger.LogInformation($"Rating deleted successfully");

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not delete rating");
            return BadRequest($"Could not delete. Error: {ex.Message}");
        }

    }


}

