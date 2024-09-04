namespace Vem.Options;

public class AuthenticationOptions
{
  public static string OptionsSectionKey = "Authentication";
  public string? Secret { get; set; }
}