using PaymentService.DataAccess.Repositories;
using PaymentService.Models;

namespace PaymentService.Business.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(
        IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<Payment?> CreatePaymentAsync(
        int orderId,
        decimal amount,
        string paymentMethod)
    {
        var payment = new Payment
        {
            OrderId = orderId,
            Amount = amount,
            PaymentMethod = paymentMethod,
            PaymentStatus = "Completed",
            PaymentDate = DateTime.UtcNow
        };

        await _paymentRepository.AddAsync(payment);

        return payment;
    }

    public async Task<Payment?> GetPaymentByIdAsync(int id)
    {
        return await _paymentRepository.GetByIdAsync(id);
    }

    public async Task<List<Payment>> GetPaymentsByOrderIdAsync(
        int orderId)
    {
        return await _paymentRepository
            .GetByOrderIdAsync(orderId);
    }
}