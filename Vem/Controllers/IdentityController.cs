using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Vem.Controllers.Models.Requests;
using Vem.Database.Contexts;
using Vem.Database.Models;
using Vem.Options;

namespace Vem.Controllers;

[ApiController]
public class AuthenticationController : ControllerBase
{
  private readonly IdentityContext identityContext;
  private readonly ApplicationSettingsContext applicationSettingsContext;
  private readonly IOptions<AuthenticationOptions> authenticationOptions;

  public AuthenticationController(IdentityContext identityContext, ApplicationSettingsContext applicationSettingsContext, IOptions<AuthenticationOptions> authenticationOptions)
  {
    this.identityContext = identityContext;
    this.applicationSettingsContext = applicationSettingsContext;
    this.authenticationOptions = authenticationOptions;
  }

  [HttpPost]
  [Route("authentication/login")]
  [ProducesResponseType<string>(StatusCodes.Status200OK)]
  public async Task<IActionResult> Login(PasswordAuthentication requestModel)
  {
    var identity = await identityContext.Identities.FirstOrDefaultAsync(identity => identity.Email == requestModel.Email);
    if (identity is null || string.IsNullOrEmpty(requestModel.Password)) return BadRequest("Invalid email or password");
    var isValid = identityContext.ValidatePassword(identity, requestModel.Password);

    return Ok(GenerateJwtToken(identity));
  }

  [HttpPost]
  [Route("authentication/validate")]
  [ProducesResponseType<string>(StatusCodes.Status200OK)]
  public IActionResult Validate()
  {
    string authHeader = Request.Headers["Authorization"];
    return Ok(ValidateJwtToken(authHeader.Replace("Bearer ", "")));
  }

  private string GenerateJwtToken(Identity identity)
  {
    // Define token handler
    var tokenHandler = new JwtSecurityTokenHandler();

    if (authenticationOptions.Value.Secret is null) throw new Exception("Secret not set");

    // Define a secret key for signing the token
    var key = Encoding.ASCII.GetBytes(authenticationOptions.Value.Secret); // Replace with your secret key

    // Define token descriptor
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(
        [
                new Claim(ClaimTypes.Name, $"{identity.FirstName} {identity.LastName}"),
                new Claim(ClaimTypes.Role, identity.IsAdmin ? "Admin" : "User")
            ]),
      Expires = DateTime.UtcNow.AddHours(1),
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };

    // Create the token
    var token = tokenHandler.CreateToken(tokenDescriptor);

    // Return the serialized token
    return tokenHandler.WriteToken(token);
  }

  private ClaimsPrincipal ValidateJwtToken(string token)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    if (authenticationOptions.Value.Secret is null) throw new Exception("Secret not set");
    var key = Encoding.ASCII.GetBytes(authenticationOptions.Value.Secret); // Replace with your secret key

    var validationParameters = new TokenValidationParameters
    {
      ValidateIssuerSigningKey = true,  // Ensure the token's signature is valid
      IssuerSigningKey = new SymmetricSecurityKey(key),  // Provide the same key used to generate the token
      ValidateIssuer = false,           // (Optional) Specify whether to validate the token's issuer
      ValidateAudience = false,         // (Optional) Specify whether to validate the token's audience
      ValidateLifetime = true,          // Ensure the token hasn't expired
      ClockSkew = TimeSpan.Zero         // Time tolerance to account for potential clock differences
    };

    // Validate the token and return the principal (claims)
    SecurityToken validatedToken;
    ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

    return principal;
  }
}