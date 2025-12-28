using EventPlanning.Application.Interfaces;
using EventPlanning.Infrastructure.Persistence;              
using EventPlanning.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("src/SmartPlatform.Api/appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"src/SmartPlatform.Api/appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Keycloak:Authority"];
        options.Audience = builder.Configuration["Keycloak:Audience"];
   options.RequireHttpsMetadata = false; // Only for dev
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
});


builder.Services.AddAuthorization();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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
builder.Services.AddScoped<IPerformerRepository, PerformerRepository>();
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

app.UseAuthentication();
app.UseAuthorization();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

//examples endpoint show how to apply middleware to the endpoints

app.MapGet("/admin", () => "Admin Access")
   .RequireAuthorization("AdminPolicy");

app.MapGet("/user", () => "User Access")
   .RequireAuthorization("UserPolicy");
app.Run();