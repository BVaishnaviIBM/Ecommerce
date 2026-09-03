using ECommerce.Business.Services;
using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(
            IPaymentService paymentService,
            ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPaymentById(int id)
        {
            _logger.LogInformation(
                "Fetching payment with PaymentId: {PaymentId}",
                id);

            var payment = await _paymentService.GetPaymentByIdAsync(id);

            if (payment == null)
            {
                _logger.LogWarning(
                    "Payment not found with PaymentId: {PaymentId}",
                    id);

                return NotFound("Payment not found.");
            }

            return Ok(payment);
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<Payment>> GetPaymentByOrderId(
            int orderId)
        {
            _logger.LogInformation(
                "Fetching payment for OrderId: {OrderId}",
                orderId);

            var payment =
                await _paymentService.GetPaymentByOrderIdAsync(orderId);

            if (payment == null)
            {
                _logger.LogWarning(
                    "Payment not found for OrderId: {OrderId}",
                    orderId);

                return NotFound("Payment not found for this order.");
            }

            return Ok(payment);
        }
    }
}