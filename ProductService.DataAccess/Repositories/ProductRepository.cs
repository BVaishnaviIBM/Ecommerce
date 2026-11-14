using Microsoft.EntityFrameworkCore;
using ProductService.DataAccess.Data;
using ProductService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductService.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductServiceDbContext _context;

        public ProductRepository(ProductServiceDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
