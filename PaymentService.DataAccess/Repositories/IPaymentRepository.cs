using PaymentService.Models;

namespace PaymentService.DataAccess.Repositories;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(int id);

    Task<List<Payment>> GetByOrderIdAsync(int orderId);

    Task AddAsync(Payment payment);

    Task UpdateAsync(Payment payment);
}