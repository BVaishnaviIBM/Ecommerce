using System;
using System.Collections.Generic;
using System.Text;

using OrderService.Models;

namespace OrderService.Business.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetMyOrdersAsync(int userId);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<Order?> CheckoutAsync(int userId, string paymentMethod);
    }
}
