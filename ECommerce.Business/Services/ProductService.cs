using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
