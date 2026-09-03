using System;
using System.Collections.Generic;
using System.Text;

using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);

        Task<User?> GetByIdAsync(int id);

        Task AddAsync(User user);

        Task<List<User>> GetAllAsync();
    }
}
