using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Vem.Database.Models;
using Vem.Options;

namespace Vem.Services;

public class TokenService(IOptions<AuthenticationOptions> authenticationOptions)
{
  private readonly AuthenticationOptions authenticationOptions = authenticationOptions.Value;

  public ClaimsPrincipal? ValidateJwtToken(string token)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    if (authenticationOptions.Secret is null) throw new Exception("Secret not set");
    var key = Encoding.ASCII.GetBytes(authenticationOptions.Secret);

    var validationParameters = new TokenValidationParameters
    {
      ValidateIssuerSigningKey = true,
      // Todo: Asymetric key encryption
      IssuerSigningKey = new SymmetricSecurityKey(key),
      ValidateIssuer = false,
      ValidateAudience = false,
      ValidateLifetime = true,
      ClockSkew = TimeSpan.Zero
    };

    SecurityToken validatedToken;
    try
    {
      ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
      return principal;
    }
    catch (SecurityTokenExpiredException)
    {
      return null;
    }
  }

  public string GenerateJwtToken(Identity identity)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    if (authenticationOptions.Secret is null) throw new Exception("Secret not set");

    var key = Encoding.ASCII.GetBytes(authenticationOptions.Secret);

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

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }
}