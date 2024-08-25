using Microsoft.AspNetCore.Mvc;
using Vem.Database.Contexts;
using Vem.Database.Models;

namespace Vem.Controllers;

[ApiController]
public class WeatherController : ControllerBase
{
  private readonly TestContext testContext;
  private readonly ApplicationSettingsContext applicationSettingsContext;

  public WeatherController(TestContext testContext, ApplicationSettingsContext applicationSettingsContext)
  {
    this.testContext = testContext;
    this.applicationSettingsContext = applicationSettingsContext;
  }

  [HttpGet]
  [Route("admin-account-initialized")]
  [ProducesResponseType<string>(StatusCodes.Status200OK)]
  public IActionResult IsInitialized()
  {
    return Ok(applicationSettingsContext.AdminAccountInitialized);
  }

  [HttpPost]
  [Route("admin-account-initialized")]
  [ProducesResponseType<string>(StatusCodes.Status200OK)]
  public IActionResult SetIsInitialized(bool value)
  {
    applicationSettingsContext.AdminAccountInitialized = value;
    return Ok();
  }

  [HttpGet]
  [Route("list")]
  [ProducesResponseType<string>(StatusCodes.Status200OK)]
  public IActionResult List()
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