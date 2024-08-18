using Microsoft.EntityFrameworkCore;
using Vem.Database.Models;

namespace Vem.Database.Contexts;

public class TestContext : DbContext
{

    public DbSet<Test> Tests { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // TODO: move to options
        optionsBuilder.UseNpgsql("Server=db;Port=5432;Database=postgres;User Id=postgres;Password=Password1234!;");
        //optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=Password1234!;");
    }

}