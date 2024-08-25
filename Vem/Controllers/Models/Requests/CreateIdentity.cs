using System.ComponentModel.DataAnnotations;

namespace Vem.Controllers.Models.Requests
{
  public class CreateIdentity
  {
    [Required]
    public string FirstName { get; set; } = nameof(FirstName);
    public string? LastName { get; set; }
    public string? Nickname { get; set; }
    public string? Email { get; set; }
  }
}