using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Vem.Database.Contexts;
using Vem.Options;

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


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
