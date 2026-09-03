using System;
using System.Collections.Generic;
using System.Text;

using ECommerce.DataAccess.Repositories;
using ECommerce.Models.Entities;

namespace ECommerce.Business.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            return await _cartRepository.GetCartByUserIdAsync(userId);
        }

        public async Task<Cart?> GetCartByIdAsync(int cartId)
        {
            return await _cartRepository.GetByIdAsync(cartId);
        }

        public async Task CreateCartAsync(Cart cart)
        {
            await _cartRepository.AddAsync(cart);
        }

        public async Task AddItemAsync(CartItem cartItem)
        {
            var existingItem = await _cartRepository.GetItemAsync(
                cartItem.CartId,
                cartItem.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += cartItem.Quantity;

                await _cartRepository.UpdateItemAsync(existingItem);
            }
            else
            {
                await _cartRepository.AddItemAsync(cartItem);
            }
        }

        public async Task UpdateItemAsync(CartItem cartItem)
        {
            await _cartRepository.UpdateItemAsync(cartItem);
        }

        public async Task RemoveItemAsync(CartItem cartItem)
        {
            await _cartRepository.RemoveItemAsync(cartItem);
        }
    }
}
