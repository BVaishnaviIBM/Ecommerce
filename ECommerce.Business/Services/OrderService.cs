using System;
using System.Collections.Generic;
using System.Text;

using ECommerce.DataAccess.Repositories;
using ECommerce.Models.Entities;

namespace ECommerce.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public OrderService(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IInventoryRepository inventoryRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<List<Order>> GetMyOrdersAsync(int userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetByIdAsync(orderId);
        }

        public async Task<Order?> CheckoutAsync(
            int userId,
            string paymentMethod)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null || !cart.CartItems.Any())
            {
                return null;
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = "Confirmed"
            };

            decimal totalAmount = 0;

            foreach (var cartItem in cart.CartItems)
            {
                var inventory = await _inventoryRepository
                    .GetByProductIdAsync(cartItem.ProductId);

                if (inventory == null ||
                    inventory.Quantity < cartItem.Quantity)
                {
                    return null;
                }

                var price = cartItem.Product!.Price;

                var orderItem = new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = price
                };

                order.OrderItems.Add(orderItem);

                totalAmount += price * cartItem.Quantity;

                inventory.Quantity -= cartItem.Quantity;

                await _inventoryRepository.UpdateAsync(inventory);
            }

            order.TotalAmount = totalAmount;

            order.Payment = new Payment
            {
                Amount = totalAmount,
                PaymentMethod = paymentMethod,
                PaymentStatus = "Paid",
                PaymentDate = DateTime.UtcNow
            };

            await _orderRepository.AddAsync(order);

            foreach (var cartItem in cart.CartItems.ToList())
            {
                await _cartRepository.RemoveItemAsync(cartItem);
            }

            return order;
        }
    }
}
