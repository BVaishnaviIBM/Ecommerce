using ECommerce.Business.Services;
using ECommerce.DataAccess.Repositories;
using ECommerce.Models.DTOs;
using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ECommerce.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task GetProductByIdAsync_ReturnsProduct()
        {
            // Arrange
            var product = new Product
            {
                ProductId = 1,
                ProductName = "Test Product",
                Description = "Test Description",
                Price = 100,
                CategoryId = 1
            };

            var mockRepository = new Mock<IProductRepository>();

            mockRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(product);

            var mockInventoryRepository = new Mock<IInventoryRepository>();

            var service = new ProductService(
                mockRepository.Object,
                mockInventoryRepository.Object,
                new MemoryCache(new MemoryCacheOptions())
            );

            // Act
            var result = await service.GetProductByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.ProductId);
            Assert.Equal("Test Product", result.ProductName);
            Assert.Equal(100, result.Price);
        }
        [Fact]
        public async Task GetProductByIdAsync_ReturnsNull_WhenProductNotFound()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();

            mockRepository
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Product?)null);

            var mockInventoryRepository = new Mock<IInventoryRepository>();

            var service = new ProductService(
                mockRepository.Object,
                mockInventoryRepository.Object,
                new MemoryCache(new MemoryCacheOptions())
            );

            // Act
            var result = await service.GetProductByIdAsync(99);

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task GetAllProductsAsync_ReturnsProducts()
        {
            // Arrange
            var products = new List<Product>
    {
        new Product
        {
            ProductId = 1,
            ProductName = "Mouse",
            Price = 500
        },
        new Product
        {
            ProductId = 2,
            ProductName = "Keyboard",
            Price = 1000
        }
    };

            var mockRepository = new Mock<IProductRepository>();

            mockRepository
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(products);

            var mockInventoryRepository = new Mock<IInventoryRepository>();

            var service = new ProductService(
                mockRepository.Object,
                mockInventoryRepository.Object,
                new MemoryCache(new MemoryCacheOptions())
            );

            // Act
            var result = await service.GetAllProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Mouse", result[0].ProductName);
            Assert.Equal("Keyboard", result[1].ProductName);
        }
        [Fact]
        public async Task GetInventoryByIdAsync_ReturnsInventory()
        {
            // Arrange
            var inventory = new Inventory
            {
                InventoryId = 1,
                ProductId = 1,
                Quantity = 50
            };

            var mockRepository = new Mock<IInventoryRepository>();

            mockRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(inventory);

            var service = new InventoryService(mockRepository.Object);

            // Act
            var result = await service.GetInventoryByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.InventoryId);
            Assert.Equal(1, result.ProductId);
            Assert.Equal(50, result.Quantity);
        }
        [Fact]
        public async Task GetInventoryByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var mockRepository = new Mock<IInventoryRepository>();

            mockRepository
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Inventory?)null);

            var service = new InventoryService(mockRepository.Object);

            // Act
            var result = await service.GetInventoryByIdAsync(99);

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task GetCartByUserIdAsync_ReturnsCart()
        {
            // Arrange
            var cart = new Cart
            {
                CartId = 1,
                UserId = 1
            };

            var mockRepository = new Mock<ICartRepository>();

            mockRepository
                .Setup(r => r.GetCartByUserIdAsync(1))
                .ReturnsAsync(cart);

            var service = new CartService(mockRepository.Object);

            // Act
            var result = await service.GetCartByUserIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.CartId);
            Assert.Equal(1, result.UserId);
        }
        [Fact]
        public async Task AddItemAsync_AddsNewItem()
        {
            // Arrange
            var cartItem = new CartItem
            {
                CartId = 1,
                ProductId = 2,
                Quantity = 3
            };

            var mockRepository = new Mock<ICartRepository>();

            mockRepository
                .Setup(r => r.GetItemAsync(1, 2))
                .ReturnsAsync((CartItem?)null);

            var service = new CartService(mockRepository.Object);

            // Act
            await service.AddItemAsync(cartItem);

            // Assert
            mockRepository.Verify(
                r => r.AddItemAsync(cartItem),
                Times.Once);
        }
        [Fact]
        public async Task CheckoutAsync_CreatesOrder_WhenCartHasItems()
        {
            // Arrange
            var product = new Product
            {
                ProductId = 1,
                ProductName = "Test Product",
                Price = 100
            };

            var cart = new Cart
            {
                CartId = 1,
                UserId = 1,
                CartItems = new List<CartItem>
        {
            new CartItem
            {
                CartItemId = 1,
                CartId = 1,
                ProductId = 1,
                Quantity = 2,
                Product = product
            }
        }
            };

            var inventory = new Inventory
            {
                InventoryId = 1,
                ProductId = 1,
                Quantity = 10
            };

            var mockOrderRepository = new Mock<IOrderRepository>();
            var mockCartRepository = new Mock<ICartRepository>();
            var mockInventoryRepository = new Mock<IInventoryRepository>();

            mockCartRepository
                .Setup(r => r.GetCartByUserIdAsync(1))
                .ReturnsAsync(cart);

            mockInventoryRepository
                .Setup(r => r.GetByProductIdAsync(1))
                .ReturnsAsync(inventory);

            var service = new OrderService(
                mockOrderRepository.Object,
                mockCartRepository.Object,
                mockInventoryRepository.Object);

            // Act
            var result = await service.CheckoutAsync(1, "UPI");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.UserId);
            Assert.Equal(200, result.TotalAmount);
            Assert.Equal("Confirmed", result.Status);

            mockOrderRepository.Verify(
                r => r.AddAsync(It.IsAny<Order>()),
                Times.Once);
        }
        [Fact]
        public async Task CheckoutAsync_ReturnsNull_WhenCartIsEmpty()
        {
            // Arrange
            var emptyCart = new Cart
            {
                CartId = 1,
                UserId = 1,
                CartItems = new List<CartItem>()
            };

            var mockOrderRepository = new Mock<IOrderRepository>();
            var mockCartRepository = new Mock<ICartRepository>();
            var mockInventoryRepository = new Mock<IInventoryRepository>();

            mockCartRepository
                .Setup(r => r.GetCartByUserIdAsync(1))
                .ReturnsAsync(emptyCart);

            var service = new OrderService(
                mockOrderRepository.Object,
                mockCartRepository.Object,
                mockInventoryRepository.Object);

            // Act
            var result = await service.CheckoutAsync(1, "UPI");

            // Assert
            Assert.Null(result);

            mockOrderRepository.Verify(
                r => r.AddAsync(It.IsAny<Order>()),
                Times.Never);
        }
        [Fact]
        public async Task RegisterAsync_CreatesUser_WhenEmailDoesNotExist()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "Test@123"
            };

            var mockRepository = new Mock<IUserRepository>();

            mockRepository
                .Setup(r => r.GetByEmailAsync(request.Email))
                .ReturnsAsync((User?)null);

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Key"] = "ECommerceCapstoneProjectSecretKey2026VerySecure",
                    ["Jwt:Issuer"] = "ECommerceAPI",
                    ["Jwt:Audience"] = "ECommerceClient"
                })
                .Build();

            var service = new UserService(
                mockRepository.Object,
                configuration);

            // Act
            var result = await service.RegisterAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test User", result.Name);
            Assert.Equal("test@example.com", result.Email);
            Assert.Equal("Customer", result.Role);

            mockRepository.Verify(
                r => r.AddAsync(It.Is<User>(u =>
                    u.Email == "test@example.com" &&
                    !string.IsNullOrEmpty(u.PasswordHash))),
                Times.Once);
        }
        [Fact]
        public async Task LoginAsync_ReturnsToken_WhenCredentialsAreCorrect()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                Name = "Test User",
                Email = "test@example.com",
                Role = "Customer"
            };

            var passwordHasher = new PasswordHasher<User>();

            user.PasswordHash = passwordHasher.HashPassword(
                user,
                "Test@123");

            var request = new LoginRequest
            {
                Email = "test@example.com",
                Password = "Test@123"
            };

            var mockRepository = new Mock<IUserRepository>();

            mockRepository
                .Setup(r => r.GetByEmailAsync(request.Email))
                .ReturnsAsync(user);

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Key"] = "ECommerceCapstoneProjectSecretKey2026VerySecure",
                    ["Jwt:Issuer"] = "ECommerceAPI",
                    ["Jwt:Audience"] = "ECommerceClient"
                })
                .Build();

            var service = new UserService(
                mockRepository.Object,
                configuration);

            // Act
            var result = await service.LoginAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test User", result.Name);
            Assert.Equal("test@example.com", result.Email);
            Assert.Equal("Customer", result.Role);
            Assert.False(string.IsNullOrEmpty(result.Token));
        }
        [Fact]
        public async Task GetPaymentByIdAsync_ReturnsPayment()
        {
            // Arrange
            var payment = new Payment
            {
                PaymentId = 1,
                OrderId = 1,
                Amount = 2399.97m,
                PaymentMethod = "UPI",
                PaymentStatus = "Paid"
            };

            var mockRepository = new Mock<IPaymentRepository>();

            mockRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(payment);

            var service = new PaymentService(mockRepository.Object);

            // Act
            var result = await service.GetPaymentByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.PaymentId);
            Assert.Equal(2399.97m, result.Amount);
            Assert.Equal("UPI", result.PaymentMethod);
            Assert.Equal("Paid", result.PaymentStatus);
        }
        [Fact]
        public async Task GetPaymentByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var mockRepository = new Mock<IPaymentRepository>();

            mockRepository
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Payment?)null);

            var service = new PaymentService(mockRepository.Object);

            // Act
            var result = await service.GetPaymentByIdAsync(99);

            // Assert
            Assert.Null(result);
        }
    }
}