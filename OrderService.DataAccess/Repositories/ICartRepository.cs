using System;
using System.Collections.Generic;
using System.Text;

using OrderService.Models;

namespace OrderService.DataAccess.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByUserIdAsync(int userId);

        Task<Cart?> GetByIdAsync(int cartId);
        Task<CartItem?> GetItemAsync(int cartId, int productId);

        Task AddAsync(Cart cart);

        Task AddItemAsync(CartItem cartItem);

        Task UpdateItemAsync(CartItem cartItem);

        Task RemoveItemAsync(CartItem cartItem);
    }
}
