using System;
using System.Collections.Generic;
using System.Text;

using ECommerce.Models.Entities;

namespace ECommerce.Business.Services
{
    public interface ICartService
    {
        Task<Cart?> GetCartByUserIdAsync(int userId);
        Task<Cart?> GetCartByIdAsync(int cartId);
        Task CreateCartAsync(Cart cart);
        Task AddItemAsync(CartItem cartItem);
        Task UpdateItemAsync(CartItem cartItem);
        Task RemoveItemAsync(CartItem cartItem);
    }
}
