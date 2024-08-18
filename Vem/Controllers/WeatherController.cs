using Microsoft.AspNetCore.Mvc;
using Vem.Database.Contexts;
using Vem.Database.Models;

namespace Vem.Controllers;

[ApiController]
public class WeatherController : ControllerBase
{
  private readonly TestContext testContext;

  public WeatherController(TestContext testContext)
  {
    this.testContext = testContext;
  }

  [HttpGet]
  [Route("list")]
  [ProducesResponseType<string>(StatusCodes.Status200OK)]
  public IActionResult List()
  {
    return Ok(testContext.Tests.ToList());
  }

  [HttpGet]
  [Route("arst")]
  [ProducesResponseType<string>(StatusCodes.Status200OK)]
  public IActionResult arst()
  {
    return Ok(testContext.Tests.ToList());
  }

  [HttpGet]
  [Route("get")]
  [ProducesResponseType<string>(StatusCodes.Status200OK)]
  public IActionResult Get([FromQuery] int id)
  {
    return Ok(testContext.Tests.FirstOrDefault(x => x.Id == id));
  }

  [HttpPost]
  [Route("add")]
  public IActionResult Add([FromBody] string name)
  {
    var test = new Test { Name = name };
    testContext.Tests.Add(test);
    testContext.SaveChanges();
    return Created();

  }
}