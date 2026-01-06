using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Security.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    public UserRepository(ApplicationDbContext context) => _context = context;

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetUserbyId(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public Task Login(User user)
    {
        // placeholder - implement authentication-related persistence if needed
        return Task.CompletedTask;
    }
}
