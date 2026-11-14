using Microsoft.AspNetCore.Mvc;
using UserService.Business.Services;
using UserService.Models.DTOs;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(
        RegisterRequest request)
    {
        var user = await _userService.RegisterAsync(
            request.Name,
            request.Email,
            request.Password,
            request.Role);

        if (user == null)
        {
            return BadRequest(
                "User with this email already exists.");
        }

        return Ok(new AuthResponse
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(
        LoginRequest request)
    {
        var response = await _userService.LoginAsync(
            request.Email,
            request.Password);

        if (response == null)
        {
            return Unauthorized(
                "Invalid email or password.");
        }

        return Ok(response);
    }
}