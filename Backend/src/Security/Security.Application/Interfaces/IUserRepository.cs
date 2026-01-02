using System;
using System.Threading.Tasks;

public interface IUserRepository
{
    Task AddAsync(User user);

    Task<User?> GetUserbyId(Guid id);

    Task Login(User user);
}