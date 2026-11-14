using PaymentService.Models;

namespace PaymentService.Business.Services;

public interface IPaymentService
{
    Task<Payment?> CreatePaymentAsync(
        int orderId,
        decimal amount,
        string paymentMethod);

    Task<Payment?> GetPaymentByIdAsync(int id);

    Task<List<Payment>> GetPaymentsByOrderIdAsync(
        int orderId);
}