using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Vem.Database.Models;
using Vem.Options;

namespace Vem.Database.Contexts;

public class ApplicationSettingsContext : DbContext
{

  public string ConnectionString { get; set; }
  public DbSet<ApplicationSetting> ApplicationSettings { get; set; }

  publ
  public bool AdminAccountInitialized

  public bool? GetBoolValue(string key)
  {
    var setting = ApplicationSettings.FirstOrDefault(s => s.Key == key);
    if (setting == null)
    {
      return null;
    }
    return bool.Parse(setting.Value);
  }

  public ApplicationSettingsContext(IOptions<PostgresqlOptions> options)
  {
    var connectionString = options.Value.ConnectionString;
    if (string.IsNullOrWhiteSpace(connectionString))
    {
      throw new ArgumentNullException(nameof(connectionString));
    }
    ConnectionString = connectionString;
  }
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseNpgsql(ConnectionString);
  }

}