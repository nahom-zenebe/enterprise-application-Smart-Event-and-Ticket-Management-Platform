using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Security.Infrastructure.Persistence;


using EventPlanning.Application.Interfaces;
using EventPlanning.Infrastructure.Persistence;              
using EventPlanning.Infrastructure.Repositories;

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

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.UseAuthentication();
app.UseAuthorization();


app.UseHttpsRedirection();

app.MapControllers();

//examples endpoint show how to apply middleware to the endpoints

app.MapGet("/admin", () => "Admin Access")
   .RequireAuthorization("AdminPolicy");

app.MapGet("/user", () => "User Access")
   .RequireAuthorization("UserPolicy");

app.Run();
