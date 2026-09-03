using System;
using System.Collections.Generic;
using System.Text;

using ECommerce.Models.Entities;

namespace ECommerce.Business.Services
{
    public interface IPaymentService
    {
        Task<Payment?> GetPaymentByIdAsync(int paymentId);
        Task<Payment?> GetPaymentByOrderIdAsync(int orderId);
        Task<List<Payment>> GetAllPaymentsAsync();
    }
}
