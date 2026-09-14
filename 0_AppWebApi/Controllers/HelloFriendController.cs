using Microsoft.AspNetCore.Mvc;

namespace AppWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloFriendController : ControllerBase
{
    [HttpGet("{name}")] // Collects the data from the URL and passes it to the Get method as a parameter
    public string Get(string name)
    {
        // https://localhost:7281/HelloFriend/stefan returns Hello stefan!
        return $"Hello {name}!";
    }


}
