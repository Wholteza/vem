using Microsoft.AspNetCore.Authorization;
using Vem.Database.Models;

namespace Vem.Authorization;
public class CustomAuthorizationHandler : AuthorizationHandler<IsAdminAuthorizationRequirement>
{
  protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAdminAuthorizationRequirement requirement)
  {
    if (context.User.Identity is null)
    {
      return Task.CompletedTask;
    }

    // TODO: Implement authorization logic
    if (false)
    {
      context.Succeed(requirement);
    }

    return Task.CompletedTask;
  }
}
