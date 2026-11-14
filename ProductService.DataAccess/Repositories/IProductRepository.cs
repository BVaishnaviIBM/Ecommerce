using System;
using System.Collections.Generic;
using System.Text;

using ProductService.Models;

namespace ProductService.DataAccess.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();

        Task<Product?> GetByIdAsync(int id);

        Task AddAsync(Product product);

        Task UpdateAsync(Product product);

        Task DeleteAsync(Product product);
    }
}
