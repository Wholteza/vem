
namespace Database.Models;

public class PasswordAuthentication : AuthenticationMethod
{
  public string? PasswordHash { get; set; }
}