using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ECommerce.Models.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ECommerce.Tests
{
    public class ProductApiIntegrationTests
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductApiIntegrationTests(
            WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task RegisterUser_ReturnsSuccessStatusCode()
        {
            // Arrange
            var request = new
            {
                Name = "Integration Test User",
                Email = $"test{Guid.NewGuid()}@example.com",
                Password = "Test@123"
            };

            // Act
            var response = await _client.PostAsJsonAsync(
                "/api/Users/register",
                request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task LoginUser_ReturnsToken()
        {
            // Arrange
            var email = $"login{Guid.NewGuid()}@example.com";

            await _client.PostAsJsonAsync(
                "/api/Users/register",
                new
                {
                    Name = "Login Test User",
                    Email = email,
                    Password = "Test@123"
                });

            var loginRequest = new
            {
                Email = email,
                Password = "Test@123"
            };

            // Act
            var response = await _client.PostAsJsonAsync(
                "/api/Users/login",
                loginRequest);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content
                .ReadFromJsonAsync<AuthResponse>();

            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.Token));
        }

        [Fact]
        public async Task GetProducts_WithoutToken_ReturnsUnauthorized()
        {
            // Act
            var response = await _client.GetAsync("/api/Products");

            // Assert
            Assert.Equal(
                HttpStatusCode.Unauthorized,
                response.StatusCode);
        }

        [Fact]
        public async Task GetProducts_WithValidToken_ReturnsSuccess()
        {
            // Arrange
            var email = $"product{Guid.NewGuid()}@example.com";

            await _client.PostAsJsonAsync(
                "/api/Users/register",
                new
                {
                    Name = "Product Test User",
                    Email = email,
                    Password = "Test@123"
                });

            var loginResponse = await _client.PostAsJsonAsync(
                "/api/Users/login",
                new
                {
                    Email = email,
                    Password = "Test@123"
                });

            var auth = await loginResponse.Content
                .ReadFromJsonAsync<AuthResponse>();

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    auth!.Token);

            // Act
            var response = await _client.GetAsync("/api/Products");

            // Assert
            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);
        }
    }
}