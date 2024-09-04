namespace Vem.Options;

public class AuthenticationOptions
{
  public static string OptionsSectionKey = "Authentication";
  public string? Salt { get; set; }
}