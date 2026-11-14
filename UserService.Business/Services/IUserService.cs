using UserService.Models;
using UserService.Models.DTOs;

namespace UserService.Business.Services;

public interface IUserService
{
    Task<User?> RegisterAsync(
        string name,
        string email,
        string password,
        string role);

    Task<AuthResponse?> LoginAsync(
        string email,
        string password);
}