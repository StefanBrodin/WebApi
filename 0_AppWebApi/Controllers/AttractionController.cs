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
}



