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
  [Route("identity")]
  [ProducesResponseType<string>(StatusCodes.Status200OK)]
  public IActionResult IsInitialized()
  {
    return Ok(applicationSettingsContext.AdminAccountInitialized);
  }

  [HttpPost]
  [Route("identity")]
  [ProducesResponseType<string>(StatusCodes.Status200OK)]
  public async Task<IActionResult> Create(CreateIdentity requestModel)
  {
    if (applicationSettingsContext.AdminAccountInitialized) return BadRequest("Admin account already initialized");

    using var transaction = await identityContext.Database.BeginTransactionAsync();
    await applicationSettingsContext.Database.UseTransactionAsync(transaction.GetDbTransaction());

    var identity = await identityContext.Identities.AddAsync(new Identity
    {
      FirstName = requestModel.FirstName,
      LastName = requestModel.LastName,
      Nickname = requestModel.Nickname,
      Email = requestModel.Email,
      IsAdmin = true
    });

    applicationSettingsContext.AdminAccountInitialized = true;

    await transaction.CommitAsync();

    return Ok(identity.Entity);
  }
}