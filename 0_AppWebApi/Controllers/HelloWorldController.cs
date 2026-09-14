using Microsoft.AspNetCore.Mvc;

namespace AppWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloWorldController : ControllerBase
{
    [HttpGet] 
    public string Get()
    {
        return "Hello World!";
    }

}
