using Microsoft.EntityFrameworkCore;
using ProductService.Models;
using System.Reflection.Emit;

namespace ProductService.DataAccess.Data;

public class ProductServiceDbContext : DbContext
{
    public ProductServiceDbContext(
        DbContextOptions<ProductServiceDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<Inventory> Inventories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);
    }
}