using Microsoft.EntityFrameworkCore;
using PaymentService.DataAccess.Data;
using PaymentService.Models;

namespace PaymentService.DataAccess.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly PaymentServiceDbContext _context;

    public PaymentRepository(
        PaymentServiceDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> GetByIdAsync(int id)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(p => p.PaymentId == id);
    }

    public async Task<List<Payment>> GetByOrderIdAsync(
        int orderId)
    {
        return await _context.Payments
            .Where(p => p.OrderId == orderId)
            .ToListAsync();
    }

    public async Task AddAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Payment payment)
    {
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync();
    }
}