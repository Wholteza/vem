using Microsoft.AspNetCore.Mvc;

namespace vem.Controllers;

[ApiController]
public class WeatherController : ControllerBase
{
  [HttpGet]
  [Route("name")]
  [ProducesResponseType<string>(StatusCodes.Status200OK)]
  public IActionResult Index([FromQuery] string name)
  {
    return Ok($"Hello {name}");
  }

  [HttpPost]
  [Route("name2")]
  public IActionResult Index2([FromBody] string name)
  {
    return Ok($"Hello {name}");
  }
}