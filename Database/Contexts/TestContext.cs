using Microsoft.EntityFrameworkCore;
using vem.Database.Models;

namespace vem.Database.Contexts;

public class TestContext : DbContext
{

    public DbSet<Test> Tests { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // TODO: move to options
        optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=Password1234!;");
    }

}