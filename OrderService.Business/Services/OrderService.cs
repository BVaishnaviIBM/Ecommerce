using OrderService.DataAccess.Repositories;
using OrderService.Models;

namespace OrderService.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;

        public OrderService(
            IOrderRepository orderRepository,
            ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }

        public async Task<List<Order>> GetMyOrdersAsync(int userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetByIdAsync(orderId);
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null || !cart.CartItems.Any())
            {
                return;
            }

            foreach (var cartItem in cart.CartItems.ToList())
            {
                await _cartRepository.RemoveItemAsync(cartItem);
            }
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

            

            throw new NotImplementedException(
                "Checkout will be implemented using Product and Payment microservices.");
        }
    }
}