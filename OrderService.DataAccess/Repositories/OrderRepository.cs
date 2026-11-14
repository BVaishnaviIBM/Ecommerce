using OrderService.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

using OrderService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderServiceDbContext _context;

        public OrderRepository(OrderServiceDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }
    }
}
