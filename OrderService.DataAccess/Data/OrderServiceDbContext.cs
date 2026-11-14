using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.DataAccess.Data;

public class OrderServiceDbContext : DbContext
{
    public OrderServiceDbContext(
        DbContextOptions<OrderServiceDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}