
public class SecurityDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public SecurityDbContext(DbContextOptions<SecurityDbContext> options) : base(options) {}
}
