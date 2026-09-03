using System;
using System.Collections.Generic;
using System.Text;

using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task<Order?> GetByIdAsync(int orderId);
        Task AddAsync(Order order);
    }
}
