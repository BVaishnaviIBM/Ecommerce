using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using UserService.DataAccess.Repositories;
using UserService.Models;
using UserService.Models.DTOs;

namespace UserService.Business.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public UserService(
        IUserRepository userRepository,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<User?> RegisterAsync(
        string name,
        string email,
        string password,
        string role)
    {
        var existingUser =
            await _userRepository.GetByEmailAsync(email);

        if (existingUser != null)
        {
            return null;
        }

        var user = new User
        {
            Name = name,
            Email = email,
            PasswordHash = HashPassword(password),
            Role = string.IsNullOrWhiteSpace(role)
                ? "Customer"
                : role
        };

        await _userRepository.AddAsync(user);

        return user;
    }

    public async Task<AuthResponse?> LoginAsync(
        string email,
        string password)
    {
        var user =
            await _userRepository.GetByEmailAsync(email);

        if (user == null)
        {
            return null;
        }

        if (!VerifyPassword(
                password,
                user.PasswordHash))
        {
            return null;
        }

        var token = GenerateJwtToken(user);

        return new AuthResponse
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            Token = token
        };
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                user.UserId.ToString()),

            new Claim(
                ClaimTypes.Name,
                user.Name),

            new Claim(
                ClaimTypes.Email,
                user.Email),

            new Claim(
                ClaimTypes.Role,
                user.Role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(
                    _configuration["Jwt:ExpiryMinutes"])),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }

    private static string HashPassword(
        string password)
    {
        byte[] salt =
            RandomNumberGenerator.GetBytes(16);

        string hash =
            Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 32));

        return $"{Convert.ToBase64String(salt)}.{hash}";
    }

    private static bool VerifyPassword(
        string password,
        string storedPassword)
    {
        var parts =
            storedPassword.Split('.');

        if (parts.Length != 2)
        {
            return false;
        }

        byte[] salt =
            Convert.FromBase64String(parts[0]);

        string hash =
            Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 32));

        return hash == parts[1];
    }
}