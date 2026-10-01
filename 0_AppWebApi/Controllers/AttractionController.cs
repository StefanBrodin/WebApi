using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
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

    // GET: /api/Attraction/Read?seeded=true&flat=true&filter=&pageNumber=0&pageSize=10
    [HttpGet("Read")]
    public async Task<ActionResult<ResponsePageDto<IAttraction>>> Read(
        [FromQuery] bool seeded = true,
        [FromQuery] bool flat = true,
        [FromQuery] string filter = null,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var result = await _service.ReadAttractionsAsync(seeded, flat, filter, pageNumber, pageSize);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading attractions page");
            return BadRequest(ex.Message);
        }
    }

    // GET: /api/Attraction/ReadItem?id=...&flat=false 
    [HttpGet("ReadItem")]
    public async Task<ActionResult<ResponseItemDto<IAttraction>>> ReadItem(
        [FromQuery] Guid id,
        [FromQuery] bool flat = false)
    {
        try
        {
            var result = await _service.ReadAttractionAsync(id, flat);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading attraction item");
            return BadRequest(ex.Message);
        }
    }

    // GET: /api/Attraction/ReadItemDto?id=...
    // Retrieves the attraction in CU-DTO format, convenient for pre-filling an update form in Swagger
    [HttpGet("ReadItemDto")]
    [ProducesResponseType(200, Type = typeof(AttractionCuDto))]
    [ProducesResponseType(400, Type = typeof(string))]
    [ProducesResponseType(404, Type = typeof(string))]
    public async Task<IActionResult> ReadItemDto([FromQuery] Guid id)
    {
        try
        {
            var result = await _service.ReadAttractionAsync(id, false);
            if (result?.Item == null) throw new ArgumentException($"Attraction {id} does not exist");

            return Ok(new AttractionCuDto(result.Item));
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading attraction DTO");
            return BadRequest(ex.Message);
        }
    }

    // GET: /api/Attraction/ReadWithoutReviews?seeded=true&filter=&pageNumber=0&pageSize=10
    [HttpGet("ReadWithoutReviews")]
    public async Task<ActionResult<ResponsePageDto<AttractionWithoutReviewsDto>>> ReadWithoutReviews(
        [FromQuery] bool seeded = true,
        [FromQuery] string filter = null,
        [FromQuery] int pageNumber = 0,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var result = await _service.ReadAttractionsWithoutReviewsAsync(seeded, filter, pageNumber, pageSize);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading attractions without reviews from SQL view");
            return BadRequest(ex.Message);
        }
    }

    // POST: /api/Attraction/CreateItem
    // Body: AttractionCuDto in JSON
    [HttpPost("CreateItem")]
    [ProducesResponseType(200, Type = typeof(ResponseItemDto<IAttraction>))]
    [ProducesResponseType(400, Type = typeof(string))]
    public async Task<IActionResult> CreateItem([FromBody] AttractionCuDto item)
    {
        try
        {
            item.EnsureValidity();
            _logger.LogInformation($"{nameof(CreateItem)}: Creating attraction {item.AttractionName}");

            var result = await _service.CreateAttractionAsync(item);
            _logger.LogInformation($"Attraction {result.Item.AttractionId} created successfully");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not create attraction");
            return BadRequest($"Could not create. Error: {ex.Message}");
        }
    }

    // PUT: /api/Attraction/UpdateItem/{id}
    // Body: AttractionCuDto in JSON
    [HttpPut("UpdateItem/{id}")]
    [ProducesResponseType(200, Type = typeof(ResponseItemDto<IAttraction>))]
    [ProducesResponseType(400, Type = typeof(string))]
    public async Task<IActionResult> UpdateItem(string id, [FromBody] AttractionCuDto item)
    {
        try
        {
            var idArg = Guid.Parse(id);
            if (item.AttractionId != null && item.AttractionId != idArg)
                throw new ArgumentException("Id mismatch between route parameter and body");

            item.AttractionId = idArg;
            item.EnsureValidity();

            _logger.LogInformation($"{nameof(UpdateItem)}: Updating attraction {idArg}");
            var result = await _service.UpdateAttractionAsync(item);
            _logger.LogInformation($"Attraction {idArg} updated successfully");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not update attraction");
            return BadRequest($"Could not update. Error: {ex.Message}");
        }
    }

    // DELETE: /api/Attraction/DeleteItem/{id}
    [HttpDelete("DeleteItem/{id}")]
    [ProducesResponseType(200, Type = typeof(ResponseItemDto<IAttraction>))]
    [ProducesResponseType(400, Type = typeof(string))]
    public async Task<IActionResult> DeleteItem(string id)
    {
        try
        {
            var idArg = Guid.Parse(id);
            _logger.LogInformation($"{nameof(DeleteItem)}: Deleting attraction {idArg}");

            var result = await _service.DeleteAttractionAsync(idArg);
            _logger.LogInformation($"Attraction {idArg} deleted successfully");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not delete attraction");
            return BadRequest($"Could not delete. Error: {ex.Message}");
        }

    }


}


