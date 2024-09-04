using Database.Models;
using Microsoft.EntityFrameworkCore;
using Vem.Database.Models;

namespace Vem.Database.Contexts;

public class IdentityContext : DbContext
{
  public DbSet<Identity> Identities { get; set; }
  public DbSet<AuthenticationMethod> AuthenticationMethods { get; set; }

  public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<Identity>().ToTable("Identities");
    modelBuilder.Entity<AuthenticationMethod>().UseTpcMappingStrategy();
    modelBuilder.Entity<PasswordAuthentication>().ToTable("PasswordAuthentications");
  }

}