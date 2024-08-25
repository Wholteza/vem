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

  public bool AdminAccountInitialized
  {
    get
    {
      var setting = ApplicationSettings.OfType<OptionalBooleanSetting>().FirstOrDefault(setting => setting.Key == nameof(AdminAccountInitialized));

      if (setting is null)
      {
        setting = new OptionalBooleanSetting(nameof(AdminAccountInitialized), false);
        ApplicationSettings.Add(setting);
        SaveChanges();
      }

      return setting.Value ?? false;

    }
    set
    {
      var setting = ApplicationSettings.OfType<OptionalBooleanSetting>().FirstOrDefault(setting => setting.Key == nameof(AdminAccountInitialized));

      if (setting is null)
      {
        setting = new OptionalBooleanSetting(nameof(AdminAccountInitialized), value);
        ApplicationSettings.Add(setting);
      }

      setting.Value = value;
      SaveChanges();
    }
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

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<ApplicationSetting>().ToTable("ApplicationSettings").HasDiscriminator<string>("Type").HasValue<OptionalBooleanSetting>(nameof(OptionalBooleanSetting));

  }

}