using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Ticketing.Infrastructure.Persistence
{
    public class TicketingDbContextFactory : IDesignTimeDbContextFactory<TicketingDbContext>
    {
        public TicketingDbContext CreateDbContext(string[] args)
        {
            // Adjusted path: Navigate up to the solution root and then to the API project
            // Assuming structure: backend/src/[Projects]
            // Run 'dotnet ef' from the backend root or adjust levels as needed
            var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "src", "SmartPlatform.Api"));

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("Postgres")
                ?? throw new InvalidOperationException("Missing connection string 'Postgres'.");

            var optionsBuilder = new DbContextOptionsBuilder<TicketingDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new TicketingDbContext(optionsBuilder.Options);
        }
    }
}