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
            // Get the path to the SmartPlatform.Api project
            var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "src", "SmartPlatform.Api"));

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("Postgres")
                ?? "Host=localhost;Port=5432;Database=SmartEventPlatformDb;Username=postgres;Password=1904";

            var optionsBuilder = new DbContextOptionsBuilder<TicketingDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new TicketingDbContext(optionsBuilder.Options);
        }
    }
}