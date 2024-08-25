namespace Vem.Database.Models;
public class Identity
{
  public int Id { get; set; }
  public string FirstName { get; set; } = nameof(FirstName);
  public string? LastName { get; set; }
  public string? Nickname { get; set; }
  public string? Email { get; set; }
  public bool IsAdmin { get; set; }

}