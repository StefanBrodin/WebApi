using Microsoft.AspNetCore.Mvc;
using DbModels;
using Services;

namespace AppWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttractionCategoryController : ControllerBase
{
    private readonly ILogger<AttractionCategoryController> _logger;
    private readonly IAttractionCategoryService _service;

    public AttractionCategoryController(ILogger<AttractionCategoryController> logger, IAttractionCategoryService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("ReadAll")]
    public async Task<ActionResult<List<AttractionCategoryDbM>>> ReadAll()
    {
        var result = await _service.ReadAllAttractionCategoriesAsync();
        return Ok(result);
    }
}



