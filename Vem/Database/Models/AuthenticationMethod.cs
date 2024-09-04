
namespace Database.Models;

public abstract class AuthenticationMethod
{
  public int Id { get; set; }
  public int IdentityId { get; set; }
}