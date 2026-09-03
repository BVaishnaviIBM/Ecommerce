using System;
using System.Collections.Generic;
using System.Text;

using ECommerce.Models.DTOs;
using ECommerce.Models.Entities;

namespace ECommerce.Business.Services
{
    public interface IUserService
    {
        Task<AuthResponse?> RegisterAsync(RegisterRequest request);

        Task<AuthResponse?> LoginAsync(LoginRequest request);

        Task<List<User>> GetAllUsersAsync();
    }
}
