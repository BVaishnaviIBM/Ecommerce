using ECommerce.Business.Services;
using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartsController> _logger;

        public CartsController(
            ICartService cartService,
            ILogger<CartsController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<Cart>> GetMyCart()
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            _logger.LogInformation(
                "Fetching cart for UserId: {UserId}",
                userId);

            var cart = await _cartService.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                _logger.LogWarning(
                    "Cart not found for UserId: {UserId}",
                    userId);

                return NotFound("Cart not found.");
            }

            return Ok(cart);
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> CreateCart()
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            _logger.LogInformation(
                "Creating cart for UserId: {UserId}",
                userId);

            var existingCart =
                await _cartService.GetCartByUserIdAsync(userId);

            if (existingCart != null)
            {
                _logger.LogWarning(
                    "Cart already exists for UserId: {UserId}",
                    userId);

                return Conflict("Cart already exists.");
            }

            var cart = new Cart
            {
                UserId = userId
            };

            await _cartService.CreateCartAsync(cart);

            _logger.LogInformation(
                "Cart created successfully with CartId: {CartId}",
                cart.CartId);

            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<ActionResult<CartItem>> AddItem(
            CartItem cartItem)
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var cart = await _cartService.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                _logger.LogWarning(
                    "Cart not found for UserId: {UserId}",
                    userId);

                return NotFound(
                    "Cart not found. Create a cart first.");
            }

            if (cart.CartId != cartItem.CartId)
            {
                _logger.LogWarning(
                    "Invalid CartId: {CartId} for UserId: {UserId}",
                    cartItem.CartId,
                    userId);

                return BadRequest("Invalid cart.");
            }

            if (cartItem.Quantity <= 0)
            {
                return BadRequest(
                    "Quantity must be greater than zero.");
            }

            _logger.LogInformation(
                "Adding ProductId: {ProductId} with Quantity: {Quantity} to CartId: {CartId}",
                cartItem.ProductId,
                cartItem.Quantity,
                cartItem.CartId);

            await _cartService.AddItemAsync(cartItem);

            return Ok(cartItem);
        }

        [HttpPut("items/{id}")]
        public async Task<ActionResult<CartItem>> UpdateItem(
            int id,
            CartItem cartItem)
        {
            if (id != cartItem.CartItemId)
            {
                return BadRequest();
            }

            if (cartItem.Quantity <= 0)
            {
                return BadRequest(
                    "Quantity must be greater than zero.");
            }

            _logger.LogInformation(
                "Updating CartItemId: {CartItemId} to Quantity: {Quantity}",
                cartItem.CartItemId,
                cartItem.Quantity);

            await _cartService.UpdateItemAsync(cartItem);

            return Ok(cartItem);
        }

        [HttpDelete("items/{id}")]
        public async Task<IActionResult> RemoveItem(int id)
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var cart = await _cartService.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                _logger.LogWarning(
                    "Cart not found for UserId: {UserId}",
                    userId);

                return NotFound("Cart not found.");
            }

            var cartItem = cart.CartItems
                .FirstOrDefault(x => x.CartItemId == id);

            if (cartItem == null)
            {
                _logger.LogWarning(
                    "CartItemId {CartItemId} not found for UserId: {UserId}",
                    id,
                    userId);

                return NotFound("Cart item not found.");
            }

            _logger.LogInformation(
                "Removing CartItemId: {CartItemId} from UserId: {UserId}",
                id,
                userId);

            await _cartService.RemoveItemAsync(cartItem);

            return NoContent();
        }
    }
}