using Microsoft.AspNetCore.Mvc;

using Services;
using Models;

namespace AppWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CarRegistrationsController : ControllerBase
{
    private readonly ICarsService _carsService;

    public CarRegistrationsController(ICarsService carsService)
    {
        _carsService = carsService;
    }

    [HttpGet(Name = "GetCarRegistration")] 
    public IEnumerable<Car> Get()
    {
        return _carsService.GetCars();
    }


}
