
namespace Database.Models;

public abstract class ApplicationSetting
{
  public int Id { get; set; }
  public string Key { get; set; } = "missing-key";
  public string? StoredValue { get; set; }
}