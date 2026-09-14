using Microsoft.AspNetCore.Mvc;

namespace AppWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloQueryController : ControllerBase
{
    [HttpGet]
    public string Get([FromQuery] string name) // Reads input from the parameter "name" in the URL: /HelloQuery?name=Johan
    {
        // https://localhost:7281/HelloQuery?name=Johan returns Hello Johan!
        return $"Hello {name}!";
    }

}
