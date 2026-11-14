using Microsoft.EntityFrameworkCore;
using UserService.DataAccess.Data;
using UserService.Models;

namespace UserService.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserServiceDbContext _context;

    public UserRepository(UserServiceDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.UserId == id);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}