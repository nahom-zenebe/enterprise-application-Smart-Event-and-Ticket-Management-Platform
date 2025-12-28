using EventPlanning.Application.Interfaces;
using EventPlanning.Infrastructure.Persistence;              
using EventPlanning.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("src/SmartPlatform.Api/appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile(
        $"src/SmartPlatform.Api/appsettings.{builder.Environment.EnvironmentName}.json",
        optional: true,
        reloadOnChange: true
    );

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var postgresConnectionString =
    builder.Configuration.GetConnectionString("Postgres")
    ?? throw new InvalidOperationException("Missing connection string 'Postgres'.");

builder.Services.AddDbContext<EventPlanningDbContext>(
    options => options.UseNpgsql(postgresConnectionString));

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<IVenueRepository, VenueRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EventPlanningDbContext>();

    try
    {
        dbContext.Database.Migrate();
        Console.WriteLine("Database migrated successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Migration failed: {ex.Message}");

        try
        {
            dbContext.Database.EnsureCreated();
            Console.WriteLine("Database created successfully.");
        }
        catch (Exception ex2)
        {
            Console.WriteLine($"Database creation failed: {ex2.Message}");
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();