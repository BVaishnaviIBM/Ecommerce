using ECommerce.Business.Services;
using ECommerce.Models.DTOs;
using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUserService userService,
            ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(
            RegisterRequest request)
        {
            _logger.LogInformation(
                "Registration attempt for email: {Email}",
                request.Email);

            var result = await _userService.RegisterAsync(request);

            if (result == null)
            {
                _logger.LogWarning(
                    "Registration failed. Email already exists: {Email}",
                    request.Email);

                return Conflict(
                    "A user with this email already exists.");
            }

            _logger.LogInformation(
                "User registered successfully with UserId: {UserId}",
                result.UserId);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(
            LoginRequest request)
        {
            _logger.LogInformation(
                "Login attempt for email: {Email}",
                request.Email);

            var result = await _userService.LoginAsync(request);

            if (result == null)
            {
                _logger.LogWarning(
                    "Login failed for email: {Email}",
                    request.Email);

                return Unauthorized(
                    "Invalid email or password.");
            }

            _logger.LogInformation(
                "User logged in successfully with UserId: {UserId}",
                result.UserId);

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            _logger.LogInformation("Fetching all users.");

            var users = await _userService.GetAllUsersAsync();

            _logger.LogInformation(
                "Retrieved {Count} users.",
                users.Count);

            return Ok(users);
        }
    }
}