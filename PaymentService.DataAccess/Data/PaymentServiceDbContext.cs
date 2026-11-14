using Microsoft.EntityFrameworkCore;
using PaymentService.Models;

namespace PaymentService.DataAccess.Data;

public class PaymentServiceDbContext : DbContext
{
    public PaymentServiceDbContext(
        DbContextOptions<PaymentServiceDbContext> options)
        : base(options)
    {
    }

    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Payment>()
            .Property(p => p.Amount)
            .HasPrecision(18, 2);
    }
}