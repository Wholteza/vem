namespace Vem.Options;

public class PostgresqlOptions
{
  public static string OptionsSectionKey = "Postgresql";
  public string? ConnectionString { get; set; }
}
