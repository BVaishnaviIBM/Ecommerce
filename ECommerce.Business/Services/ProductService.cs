
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using ECommerce.DataAccess.Repositories;
using ECommerce.Models.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace ECommerce.Business.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMemoryCache _cache;

        public ProductService(
            IProductRepository productRepository,
            IInventoryRepository inventoryRepository,
            IMemoryCache cache)
        {
            _productRepository = productRepository;
            _inventoryRepository = inventoryRepository;
            _cache = cache;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            const string cacheKey = "products";

            if (_cache.TryGetValue(
                cacheKey, out List<Product>? products))
            {
                return products!;
            }

            products = await _productRepository.GetAllAsync();

            _cache.Set(
                cacheKey,
                products,
                TimeSpan.FromMinutes(5));

            return products;
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task AddProductAsync(Product product)
        {
            
            await _productRepository.AddAsync(product);

            
            var existingInventory =
                await _inventoryRepository.GetByProductIdAsync(
                    product.ProductId);

            if (existingInventory == null)
            {
                var inventory = new Inventory
                {
                    ProductId = product.ProductId,
                    Quantity = 20
                };

                await _inventoryRepository.AddAsync(inventory);
            }

            
            _cache.Remove("products");
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _productRepository.UpdateAsync(product);
            _cache.Remove("products");
        }

        public async Task DeleteProductAsync(Product product)
        {
            await _productRepository.DeleteAsync(product);
            _cache.Remove("products");
        }

        public async Task<int> BulkAddProductsAsync(Stream excelFile)
        {
            var products = new List<Product>();

            using var workbook = new XLWorkbook(excelFile);
            var worksheet = workbook.Worksheet(1);
            var rows = worksheet.RangeUsed()?.RowsUsed();

            if (rows == null)
            {
                return 0;
            }

            
            foreach (var row in rows.Skip(1))
            {
                var product = new Product
                {
                    ProductName = row.Cell(1).GetString(),
                    Description = row.Cell(2).GetString(),
                    Price = row.Cell(3).GetValue<decimal>(),
                    CategoryId = row.Cell(4).GetValue<int>()
                };

                products.Add(product);
            }

            
            const int batchSize = 100;
            int totalAdded = 0;

            for (int i = 0; i < products.Count; i += batchSize)
            {
                var batch = products
                    .Skip(i)
                    .Take(batchSize)
                    .ToList();

                foreach (var product in batch)
                {
                    
                    await AddProductAsync(product);
                    totalAdded++;
                }
            }

            return totalAdded;
        }
    }
}