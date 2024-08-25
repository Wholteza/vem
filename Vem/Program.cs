using Vem.Database.Contexts;
using Vem.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin()));

builder.Services.AddScoped<TestContext>();
builder.Services.AddScoped<ApplicationSettingsContext>();

builder.Services.AddOptions<PostgresqlOptions>().Bind(builder.Configuration.GetSection(PostgresqlOptions.OptionsSectionKey));

builder.Configuration.AddUserSecrets(System.Reflection.Assembly.GetExecutingAssembly());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
