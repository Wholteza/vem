using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Vem.Database.Models;
using Vem.Options;

namespace Vem.Database.Contexts;

public class TestContext : DbContext
{

  public string ConnectionString { get; set; }
  public DbSet<Test> Tests { get; set; }

  public TestContext(IOptions<PostgresqlOptions> options)
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