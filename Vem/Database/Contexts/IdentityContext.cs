using System.Text;
using Database.Models;
using Isopoh.Cryptography.Argon2;
using Isopoh.Cryptography.SecureArray;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Vem.Database.Models;
using Vem.Options;

namespace Vem.Database.Contexts;

public class IdentityContext : DbContext
{
  private readonly AuthenticationOptions authenticationOptions;

  public DbSet<Identity> Identities { get; set; }
  public DbSet<AuthenticationMethod> AuthenticationMethods { get; set; }

  public IdentityContext(DbContextOptions<IdentityContext> options, AuthenticationOptions authenticationOptions) : base(options)
  {
    this.authenticationOptions = authenticationOptions;
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<Identity>().ToTable("Identities");
    modelBuilder.Entity<AuthenticationMethod>().UseTpcMappingStrategy();
    modelBuilder.Entity<PasswordAuthentication>().ToTable("PasswordAuthentications");
  }

  public async Task<Identity> CreateIdentityWithPasswordAuthentication(Identity identity, string password)
  {
    await Identities.AddAsync(identity);
    await SaveChangesAsync();
    if (identity.Id == 0) throw new Exception("Identity not created");

    var salt = GenerateSalt();

    await AuthenticationMethods.AddAsync(new PasswordAuthentication
    {
      IdentityId = identity.Id,
      Salt = salt,
      PasswordHash = GenerateHash(password, salt)
    });
    return identity;
  }

  public bool ValidatePassword(Identity identity, string password)
  {
    var authenticationMethod = AuthenticationMethods.OfType<PasswordAuthentication>().FirstOrDefault(authenticationMethod => authenticationMethod.IdentityId == identity.Id);

    if (authenticationMethod == null) return false;
    if (string.IsNullOrEmpty(authenticationMethod.Salt)) return false;

    return authenticationMethod.PasswordHash == GenerateHash(password, authenticationMethod.Salt);
  }

  private static string GenerateSalt() => Guid.NewGuid().ToString();

  private string GenerateHash(string password, string salt)
  {
    if (string.IsNullOrEmpty(authenticationOptions.Secret)) throw new Exception("Secret not set");

    var config = new Argon2Config
    {
      Type = Argon2Type.HybridAddressing,
      Version = Argon2Version.Nineteen,
      TimeCost = 4,
      MemoryCost = 1 << 16,
      Lanes = Environment.ProcessorCount,
      Threads = Environment.ProcessorCount,
      Salt = Encoding.UTF8.GetBytes(salt),
      Secret = Encoding.UTF8.GetBytes(authenticationOptions.Secret),
      HashLength = 32,
      Password = Encoding.UTF8.GetBytes(password)
    };

    using var argon2 = new Argon2(config);
    using SecureArray<byte> hashA = argon2.Hash();
    return config.EncodeString(hashA.Buffer);
  }

}