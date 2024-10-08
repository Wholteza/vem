using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Vem.Database.Contexts;
using Vem.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin()));

builder.Configuration.AddUserSecrets(System.Reflection.Assembly.GetExecutingAssembly());

DbConnection dbConnection = new NpgsqlConnection(builder.Configuration.GetSection(PostgresqlOptions.OptionsSectionKey).Get<PostgresqlOptions>()?.ConnectionString);
DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseNpgsql(builder.Configuration.GetSection(PostgresqlOptions.OptionsSectionKey).Get<PostgresqlOptions>()?.ConnectionString);
builder.Services.AddScoped(provider => new TestContext(new DbContextOptionsBuilder<TestContext>().UseNpgsql(dbConnection).Options));
builder.Services.AddScoped(provider => new ApplicationSettingsContext(new DbContextOptionsBuilder<ApplicationSettingsContext>().UseNpgsql(dbConnection).Options));
builder.Services.AddScoped(provider => new IdentityContext(new DbContextOptionsBuilder<IdentityContext>().UseNpgsql(dbConnection).Options));


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
