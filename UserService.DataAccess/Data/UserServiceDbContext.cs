using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.DataAccess.Data;

public class UserServiceDbContext : DbContext
{
    public UserServiceDbContext(
        DbContextOptions<UserServiceDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}