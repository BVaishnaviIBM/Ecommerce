using Microsoft.AspNetCore.Mvc;
using PaymentService.Business.Services;
using PaymentService.Models;
using PaymentService.Models.DTOs;

namespace PaymentService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(
        IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<ActionResult<Payment>> CreatePayment(
    CreatePaymentRequest request)
    {
        var payment = await _paymentService.CreatePaymentAsync(
            request.OrderId,
            request.Amount,
            request.PaymentMethod);

        return Ok(payment);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Payment>> GetPaymentById(
        int id)
    {
        var payment =
            await _paymentService.GetPaymentByIdAsync(id);

        if (payment == null)
        {
            return NotFound();
        }

        return Ok(payment);
    }

    [HttpGet("order/{orderId}")]
    public async Task<ActionResult<List<Payment>>>
        GetPaymentsByOrderId(int orderId)
    {
        var payments =
            await _paymentService
                .GetPaymentsByOrderIdAsync(orderId);

        return Ok(payments);
    }
}