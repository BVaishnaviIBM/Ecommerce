using System;
using System.Collections.Generic;
using System.Text;
using ClosedXML.Excel;
using ECommerce.Models.DTOs;

using ECommerce.DataAccess.Repositories;
using ECommerce.Models.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace ECommerce.Business.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMemoryCache _cache;

        public ProductService(
    IProductRepository productRepository,
    IMemoryCache cache)
        {
            _productRepository = productRepository;
            _cache = cache;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            const string cacheKey = "products";

            if (_cache.TryGetValue(cacheKey, out List<Product>? products))
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
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(Product product)
        {
            await _productRepository.DeleteAsync(product);
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

            // Read products from Excel
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

            // Process products in batches
            int batchSize = 100;
            int totalAdded = 0;

            for (int i = 0; i < products.Count; i += batchSize)
            {
                var batch = products
                    .Skip(i)
                    .Take(batchSize)
                    .ToList();

                foreach (var product in batch)
                {
                    await _productRepository.AddAsync(product);
                    totalAdded++;
                }
            }

            return totalAdded;
        }
    }
}
