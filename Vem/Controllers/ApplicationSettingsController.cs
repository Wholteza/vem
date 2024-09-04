using Microsoft.AspNetCore.Mvc;
using Vem.Database.Contexts;

namespace Vem.Controllers;

[ApiController]
public class ApplicationSettingsController : ControllerBase
{
  private readonly ApplicationSettingsContext applicationSettingsContext;

  public ApplicationSettingsController(ApplicationSettingsContext applicationSettingsContext)
  {
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
}