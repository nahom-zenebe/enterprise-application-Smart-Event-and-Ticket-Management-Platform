using Microsoft.EntityFrameworkCore;
namespace Security.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Example DbSet
    public DbSet<User> Users { get; set; } = null!;
}
