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
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "src", "SmartPlatform.Api"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("Postgres")
                ?? throw new InvalidOperationException("Missing connection string 'Postgres'.");

            var optionsBuilder = new DbContextOptionsBuilder<TicketingDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new TicketingDbContext(optionsBuilder.Options);
        }
    }
}