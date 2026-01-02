using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ---------------- MIDDLEWARE ORDER ----------------
app.UseAuthentication();
app.UseAuthorization();

// ---------------- TEST ENDPOINTS ----------------
app.MapGet("/buyer", () => "Buyer Access OK")
   .RequireAuthorization("BuyerPolicy");

app.MapGet("/seller", () => "Seller Access OK")
   .RequireAuthorization("SellerPolicy");

app.MapGet("/admin", () => "Admin Access OK")
   .RequireAuthorization("AdminPolicy");

app.Run();
