using System.Data.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Vem.Authentication;
using Vem.Authorization;
using Vem.Database.Contexts;
using Vem.Options;
using Vem.Services;
using AuthenticationOptions = Vem.Options.AuthenticationOptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme { In = Microsoft.OpenApi.Models.ParameterLocation.Header, Description = "Please enter into field the word 'Bearer' following by space", Name = "Authorization" });

  c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
  {
    {
      new Microsoft.OpenApi.Models.OpenApiSecurityScheme
      {
        Reference = new Microsoft.OpenApi.Models.OpenApiReference { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" }
      },
      new string[] { }
    }
  });

});
builder.Services.AddControllers();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin()));
builder.Configuration.AddUserSecrets(System.Reflection.Assembly.GetExecutingAssembly());

builder.Services.Configure<AuthenticationOptions>(builder.Configuration.GetSection(AuthenticationOptions.OptionsSectionKey));

DbConnection dbConnection = new NpgsqlConnection(builder.Configuration.GetSection(PostgresqlOptions.OptionsSectionKey).Get<PostgresqlOptions>()?.ConnectionString);
DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseNpgsql(builder.Configuration.GetSection(PostgresqlOptions.OptionsSectionKey).Get<PostgresqlOptions>()?.ConnectionString);
builder.Services.AddScoped(provider => new ApplicationSettingsContext(new DbContextOptionsBuilder<ApplicationSettingsContext>().UseNpgsql(dbConnection).Options));
builder.Services.AddScoped(provider => new IdentityContext(new DbContextOptionsBuilder<IdentityContext>().UseNpgsql(dbConnection).Options, builder.Configuration.GetSection(AuthenticationOptions.OptionsSectionKey).Get<AuthenticationOptions>()));

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<CustomAuthorizationHandler>();

// Add authorization and configure policy
builder.Services.AddAuthorization(options =>
{
  options.AddPolicy("CustomPolicy", policy =>
              policy.Requirements.Add(new IsAdminAuthorizationRequirement()));
});

// Register the custom handler
builder.Services.AddSingleton<IAuthorizationHandler, CustomAuthorizationHandler>();

// Add other necessary services like authentication, etc.
builder.Services.AddAuthentication("CustomScheme")
    .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("CustomScheme", null);

var app = builder.Build();

app.UseAuthorization();
app.UseAuthentication();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
