using Database.Models;
using Microsoft.EntityFrameworkCore;
using Vem.Database.Models;

namespace Vem.Database.Contexts;

public class ApplicationSettingsContext : DbContext
{

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

  public ApplicationSettingsContext(DbContextOptions<ApplicationSettingsContext> options) : base(options)
  {
  }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<ApplicationSetting>().ToTable("ApplicationSettings").HasDiscriminator<string>("Type").HasValue<OptionalBooleanSetting>(nameof(OptionalBooleanSetting));

  }

}