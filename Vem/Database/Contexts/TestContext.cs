using Microsoft.EntityFrameworkCore;
using Vem.Database.Models;

namespace Vem.Database.Contexts;

public class TestContext : DbContext
{

  public DbSet<Test> Tests { get; set; }

  public TestContext(DbContextOptions<TestContext> options) : base(options)
  {
  }
}