using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Vem.Services;

namespace Vem.Authentication;
public class CustomAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
    UrlEncoder encoder, ISystemClock clock, TokenService tokenService) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder, clock)
{
  protected override Task<AuthenticateResult> HandleAuthenticateAsync()
  {
    if (!Request.Headers.ContainsKey("Authorization"))
    {
      return Task.FromResult(AuthenticateResult.Fail("Authorization header not found."));
    }

    var authHeader = Request.Headers["Authorization"].ToString();
    if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
    {
      return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization header format."));
    }

    var token = authHeader["Bearer ".Length..].Trim();

    var claimsPrincipal = tokenService.ValidateJwtToken(token);
    if (claimsPrincipal is null)
    {
      return Task.FromResult(AuthenticateResult.Fail("Invalid token."));
    }

    var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value),
        };
    var identity = new ClaimsIdentity(claims, Scheme.Name);
    var principal = new ClaimsPrincipal(identity);

    var ticket = new AuthenticationTicket(principal, Scheme.Name);
    return Task.FromResult(AuthenticateResult.Success(ticket));
  }
}