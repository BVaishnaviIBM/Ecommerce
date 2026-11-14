using System;
using System.Collections.Generic;
using System.Text;

using ProductService.Models;

namespace ProductService.DataAccess.Repositories
{
    public interface IInventoryRepository
    {
        Task<List<Inventory>> GetAllAsync();

        Task<Inventory?> GetByIdAsync(int id);
        Task<Inventory?> GetByProductIdAsync(int productId);

        Task AddAsync(Inventory inventory);

        Task UpdateAsync(Inventory inventory);
    }
}
