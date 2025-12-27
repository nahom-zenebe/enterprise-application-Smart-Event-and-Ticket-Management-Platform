public class UserRepository : IUserRepository
{
    private readonly SecurityDbContext _context;
    public UserRepository(SecurityDbContext context) => _context = context;
}
