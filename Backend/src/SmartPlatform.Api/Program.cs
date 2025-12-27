using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("src/SmartPlatform.Api/appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"src/SmartPlatform.Api/appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();

var postgresConnectionString = builder.Configuration.GetConnectionString("Postgres")
    ?? throw new InvalidOperationException("Missing connection string 'Postgres'.");

builder.Services.AddDbContext<EventPlanningDbContext>(options =>
    options.UseNpgsql(postgresConnectionString));
builder.Services.AddScoped<IEventRepository, EventRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var eventDb = scope.ServiceProvider.GetRequiredService<EventPlanningDbContext>();
    await eventDb.Database.EnsureCreatedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
