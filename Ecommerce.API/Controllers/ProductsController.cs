using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using ECommerce.Business.Services;
using ECommerce.Models.Entities;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
            ProductService productService,
            ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            _logger.LogInformation("Fetching all products.");

            var products = await _productService.GetAllProductsAsync();

            _logger.LogInformation(
                "Retrieved {Count} products.",
                products.Count);

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            _logger.LogInformation(
                "Fetching product with ProductId: {ProductId}",
                id);

            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                _logger.LogWarning(
                    "Product not found with ProductId: {ProductId}",
                    id);

                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            _logger.LogInformation(
                "Adding new product: {ProductName}",
                product.ProductName);

            await _productService.AddProductAsync(product);

            _logger.LogInformation(
                "Product added successfully with ProductId: {ProductId}",
                product.ProductId);

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(
            int id,
            Product product)
        {
            if (id != product.ProductId)
            {
                _logger.LogWarning(
                    "Product update failed. Route ID {RouteId} does not match ProductId {ProductId}.",
                    id,
                    product.ProductId);

                return BadRequest();
            }

            _logger.LogInformation(
                "Updating product with ProductId: {ProductId}",
                id);

            await _productService.UpdateProductAsync(product);

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            _logger.LogInformation(
                "Deleting product with ProductId: {ProductId}",
                id);

            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                _logger.LogWarning(
                    "Product not found for deletion with ProductId: {ProductId}",
                    id);

                return NotFound();
            }

            await _productService.DeleteProductAsync(product);

            _logger.LogInformation(
                "Product deleted successfully with ProductId: {ProductId}",
                id);

            return NoContent();
        }
    }
}