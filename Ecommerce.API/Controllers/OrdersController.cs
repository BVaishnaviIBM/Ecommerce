using ECommerce.Business.Services;
using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ECommerce.API.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;
        private readonly IHubContext<OrderHub> _hubContext;

        public OrdersController(
    IOrderService orderService,
    ILogger<OrdersController> logger,
    IHubContext<OrderHub> hubContext)
        {
            _orderService = orderService;
            _logger = logger;
            _hubContext = hubContext;
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<Order>> Checkout(
            string paymentMethod)
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            _logger.LogInformation(
                "Checkout started for UserId: {UserId}",
                userId);


            var order = await _orderService.CheckoutAsync(
                userId,
                paymentMethod);

            if (order == null)
            {
                _logger.LogWarning(
                    "Checkout failed for UserId: {UserId}",
                    userId);

                return BadRequest(
                    "Cart is empty or there is insufficient inventory.");
            }

            _logger.LogInformation(
                "Order {OrderId} created successfully for UserId: {UserId}",
                order.OrderId,
                userId);
            await _hubContext.Clients.All.SendAsync(
    "OrderUpdated",
    order.OrderId,
    order.Status);

            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetMyOrders()
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var orders = await _orderService.GetMyOrdersAsync(userId);

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
    }
}