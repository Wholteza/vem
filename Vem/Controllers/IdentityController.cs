using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Vem.Controllers.Models.Requests;
using Vem.Database.Contexts;
using Vem.Database.Models;

namespace Vem.Controllers;

[ApiController]
public class IdentityController : ControllerBase
{
  private readonly IdentityContext identityContext;
  private readonly ApplicationSettingsContext applicationSettingsContext;

  public IdentityController(IdentityContext identityContext, ApplicationSettingsContext applicationSettingsContext)
  {
    this.identityContext = identityContext;
    this.applicationSettingsContext = applicationSettingsContext;
  }

  [HttpGet]
  [Route("identity/admin/initialized-status")]
  [ProducesResponseType<string>(StatusCodes.Status200OK)]
  public IActionResult IsInitialized()
  {
    return Ok(applicationSettingsContext.AdminAccountInitialized);
  }

  [HttpPost]
  [Route("identity/admin")]
  [ProducesResponseType<string>(StatusCodes.Status200OK)]
  public async Task<IActionResult> CreateAdminIdentity(CreateIdentity requestModel)
  {
    if (applicationSettingsContext.AdminAccountInitialized) return BadRequest("Admin account already initialized");

    using var transaction = await identityContext.Database.BeginTransactionAsync();
    await applicationSettingsContext.Database.UseTransactionAsync(transaction.GetDbTransaction());

    var identity = await identityContext.CreateIdentityWithPasswordAuthentication(new Identity
    {
      FirstName = requestModel.FirstName,
      LastName = requestModel.LastName,
      Nickname = requestModel.Nickname,
      Email = requestModel.Email,
      IsAdmin = true
    }, requestModel.Password);

    applicationSettingsContext.AdminAccountInitialized = true;

    identityContext.SaveChanges();

    await transaction.CommitAsync();

    return Ok(identity);
  }

  [HttpPost]
  [Route("identity")]
  [ProducesResponseType<string>(StatusCodes.Status200OK)]
  public async Task<IActionResult> CreateIdentity(CreateIdentity requestModel)
  {
    // check token and that the account is an admin account
    if (applicationSettingsContext.AdminAccountInitialized) return BadRequest("Admin account already initialized");

    using var transaction = await identityContext.Database.BeginTransactionAsync();
    await applicationSettingsContext.Database.UseTransactionAsync(transaction.GetDbTransaction());

    var identity = await identityContext.CreateIdentityWithPasswordAuthentication(new Identity
    {
      FirstName = requestModel.FirstName,
      LastName = requestModel.LastName,
      Nickname = requestModel.Nickname,
      Email = requestModel.Email,
      IsAdmin = false
    }, requestModel.Password);

    applicationSettingsContext.AdminAccountInitialized = true;

    identityContext.SaveChanges();

    await transaction.CommitAsync();

    return Ok(identity);
  }
}