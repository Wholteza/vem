using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Vem.Controllers.Models.Requests;
using Vem.Controllers.Models.Response;
using Vem.Database.Contexts;
using Vem.Options;
using Vem.Services;

namespace Vem.Controllers;

[ApiController]
public class AuthenticationController(
  IdentityContext identityContext, TokenService tokenService) : ControllerBase
{
  private readonly IdentityContext identityContext = identityContext;
  private readonly TokenService tokenService = tokenService;

  [HttpPost]
  [Route("authentication/login")]
  [ProducesResponseType<string>(StatusCodes.Status200OK)]
  public async Task<IActionResult> Login(PasswordAuthentication requestModel)
  {
    var identity = await identityContext.Identities.FirstOrDefaultAsync(identity => identity.Email == requestModel.Email);
    if (identity is null || string.IsNullOrEmpty(requestModel.Password)) return BadRequest("Invalid email or password");
    var isValid = identityContext.ValidatePassword(identity, requestModel.Password);

    return Ok(tokenService.GenerateJwtToken(identity));
  }

  [HttpPost]
  [Route("authentication/validate")]
  [ProducesResponseType<string>(StatusCodes.Status200OK)]
  public IActionResult Validate()
  {
    string authHeader = Request.Headers["Authorization"].FirstOrDefault() ?? "";
    var claimsPricipal = tokenService.ValidateJwtToken(authHeader.Replace("Bearer ", ""));
    var validUntil = claimsPricipal?.Claims.FirstOrDefault(claim => claim.Type == "exp")?.Value;
    var issuedAt = claimsPricipal?.Claims.FirstOrDefault(claim => claim.Type == "iat")?.Value;
    return Ok(new TokenValidation
    {
      IsValid = claimsPricipal is not null,
      ValidUntil = validUntil is not null ? new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(long.Parse(validUntil)) : null,
      IssuedAt = issuedAt is not null ? new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(long.Parse(issuedAt)) : null
    });
  }
}