using Microsoft.EntityFrameworkCore;
using Vem.Database.Models;

namespace Vem.Database.Contexts;

public class IdentityContext : DbContext
{
  public DbSet<Identity> Identities { get; set; }

  public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<Identity>().ToTable("Identities");

  }

}