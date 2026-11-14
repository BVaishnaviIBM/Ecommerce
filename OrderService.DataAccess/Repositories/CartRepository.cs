using System;
using System.Collections.Generic;
using System.Text;

using OrderService.DataAccess.Data;
using OrderService.Models;
using Microsoft.EntityFrameworkCore;

namespace OrderService.DataAccess.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly OrderServiceDbContext _context;

        public CartRepository(OrderServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart?> GetByIdAsync(int cartId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                
                .FirstOrDefaultAsync(c => c.CartId == cartId);
        }

        public async Task AddAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task AddItemAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateItemAsync(CartItem cartItem)
        {
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(
                    item => item.CartItemId == cartItem.CartItemId);

            if (existingItem == null)
            {
                throw new KeyNotFoundException(
                    $"Cart item with ID {cartItem.CartItemId} was not found.");
            }

            existingItem.Quantity = cartItem.Quantity;

            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
        public async Task<CartItem?> GetItemAsync(int cartId, int productId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(x =>
                    x.CartId == cartId &&
                    x.ProductId == productId);
        }
    }
}
