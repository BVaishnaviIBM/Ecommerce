using Microsoft.EntityFrameworkCore;
using ProductService.DataAccess.Data;
using ProductService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductService.DataAccess.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ProductServiceDbContext _context;

        public InventoryRepository(ProductServiceDbContext context)
        {
            _context = context;
        }

        public async Task<List<Inventory>> GetAllAsync()
        {
            return await _context.Inventories
                .Include(i => i.Product)
                .ToListAsync();
        }

        public async Task<Inventory?> GetByIdAsync(int id)
        {
            return await _context.Inventories
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.InventoryId == id);
        }

        public async Task AddAsync(Inventory inventory)
        {
            await _context.Inventories.AddAsync(inventory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Inventory inventory)
        {
            _context.Inventories.Update(inventory);
            await _context.SaveChangesAsync();
        }
        public async Task<Inventory?> GetByProductIdAsync(int productId)
        {
            return await _context.Inventories
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.ProductId == productId);
        }
    }
}
