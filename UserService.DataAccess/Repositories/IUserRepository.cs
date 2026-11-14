using UserService.Models;

namespace UserService.DataAccess.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetByIdAsync(int id);

    Task AddAsync(User user);
}