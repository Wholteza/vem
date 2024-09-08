using System.ComponentModel.DataAnnotations;

namespace Vem.Controllers.Models.Requests
{
  public class PasswordAuthentication
  {
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
  }
}