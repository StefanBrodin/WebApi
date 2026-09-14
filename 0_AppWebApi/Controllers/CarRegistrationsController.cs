using Microsoft.AspNetCore.Mvc;

using Services;
using Models;

namespace AppWebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CarRegistrationsController : ControllerBase
{
    private readonly ICarsService _carsService;

    public CarRegistrationsController(ICarsService carsService)
    {
        _carsService = carsService;
    }

    [HttpGet(Name = "GetCarRegistrations")] 
    public IEnumerable<Car> GetCarRegistrations()
    {
        return _carsService.GetCars();
    }


}
