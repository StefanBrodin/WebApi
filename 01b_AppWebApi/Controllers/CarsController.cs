using Microsoft.AspNetCore.Mvc;

namespace AppWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly ICarService _carService;

    // Send the ICarService instance to the controller through dependency injection
    public CarsController(ICarService carService)
    {
        _carService = carService;
    }

    // GET: api/cars
    [HttpGet]
    public ActionResult<IEnumerable<ICar>> GetCars()
    {
        var cars = _carService.GetCars();
        return Ok(cars);
    }
}