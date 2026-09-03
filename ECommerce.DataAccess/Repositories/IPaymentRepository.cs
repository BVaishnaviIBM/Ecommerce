using System;
using System.Collections.Generic;
using System.Text;

using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(int paymentId);
        Task<Payment?> GetByOrderIdAsync(int orderId);
        Task<List<Payment>> GetAllAsync();
    }
}
