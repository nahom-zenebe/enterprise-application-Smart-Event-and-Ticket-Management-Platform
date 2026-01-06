using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using EventPlanning.Application.Interfaces;
using EventPlanning.Infrastructure.Persistence;              
using EventPlanning.Infrastructure.Repositories;
using Ticketing.Infrastructure.Persistence;
using Ticketing.Infrastructure.Repositories;
using Ticketing.Application.Interfaces;
using Ticketing.Application.Commands;
using Shared.Infrastructure.Payments.Persistence;
using Shared.Infrastructure.Payments.Repositories;
using Shared.Application.Payments.Interfaces;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Text.Json.Serialization;   // ✅ ADD THIS

var builder = WebApplication.CreateBuilder(args);

// Get connection string
var postgresConnectionString = builder.Configuration.GetConnectionString("Postgres");

// ---------------- AUTHENTICATION ----------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.Authority = "http://localhost:8080/realms/dotnet-realm";
    options.RequireHttpsMetadata = false;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = "http://localhost:8080/realms/dotnet-realm",

        ValidateAudience = true,
        ValidAudience = "dotnet-api",

        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(2),

        RoleClaimType = ClaimTypes.Role
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            Console.WriteLine(
                context.Request.Headers.Authorization.ToString().StartsWith("Bearer ")
                    ? "✅ Token received in header: Yes"
                    : "❌ Token received in header: No"
            );
            return Task.CompletedTask;
        },

        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("❌ AUTH FAILED");
            Console.WriteLine(context.Exception.Message);
            return Task.CompletedTask;
        },

        OnTokenValidated = context =>
        {
            Console.WriteLine("✅ TOKEN VALIDATED SUCCESSFULLY");
            return Task.CompletedTask;
        }
    };
});

// ---------------- CLAIMS TRANSFORMATION ----------------
builder.Services.AddScoped<IClaimsTransformation, KeycloakRoleClaimsTransformation>();

// ---------------- AUTHORIZATION ----------------
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", p => p.RequireRole("Admin"));
    options.AddPolicy("BuyerPolicy", p => p.RequireRole("Buyer"));
    options.AddPolicy("SellerPolicy", p => p.RequireRole("Seller"));
});

// ✅ THIS IS THE IMPORTANT FIX
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------------- DATABASE CONTEXTS ----------------
builder.Services.AddDbContext<TicketingDbContext>(options =>
    options.UseNpgsql(postgresConnectionString));

builder.Services.AddDbContext<EventPlanningDbContext>(
    options => options.UseNpgsql(postgresConnectionString));

builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseNpgsql(postgresConnectionString));

// ---------------- REPOSITORY SERVICES ----------------
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<IVenueRepository, VenueRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

// ---------------- TICKETING SERVICES ----------------
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IReservationService, ReservationCommandHandlers>();
builder.Services.AddScoped<ITicketService, TicketCommandHandlers>();
builder.Services.AddScoped<Ticketing.Application.Interfaces.IOutboxService, Ticketing.Infrastructure.Services.OutboxService>();
builder.Services.AddScoped<Ticketing.Application.Interfaces.IEventPublisher, Ticketing.Infrastructure.Services.RabbitMQEventPublisher>();

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

// ---------------- MIDDLEWARE ORDER ----------------
app.UseAuthentication();
app.UseAuthorization();

// ---------------- ENDPOINTS ----------------
app.MapControllers();
app.Run();
