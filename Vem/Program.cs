using Vem.Database.Contexts;
using Vem.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin()));
builder.Services.AddScoped<TestContext>();
builder.Services.AddOptions<PostgresqlOptions>().Bind(builder.Configuration.GetSection(PostgresqlOptions.OptionsSectionKey));


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
