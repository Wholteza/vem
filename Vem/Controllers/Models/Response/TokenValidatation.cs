namespace Vem.Controllers.Models.Response
{
  public class TokenValidation
  {
    public bool IsValid { get; set; }
    public DateTime? ValidUntil { get; set; }
    public DateTime? IssuedAt { get; set; }

  }
}