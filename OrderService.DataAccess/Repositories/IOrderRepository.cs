using System;
using System.Collections.Generic;
using System.Text;

using OrderService.Models;

namespace OrderService.DataAccess.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task<Order?> GetByIdAsync(int orderId);
        Task AddAsync(Order order);
    }
}
