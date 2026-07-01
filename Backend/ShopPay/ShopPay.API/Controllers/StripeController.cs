using Microsoft.AspNetCore.Mvc;
using ShopPay.Application.DTOs.Stripe;
using ShopPay.Application.Interfaces;

namespace ShopPay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StripeController : ControllerBase
{
    private readonly IStripeService _stripeService;

    public StripeController(
        IStripeService stripeService)
    {
        _stripeService = stripeService;
    }

    [HttpPost("create-payment-intent")]
    public async Task<IActionResult> CreatePaymentIntent(
        [FromBody] CreatePaymentIntentRequestDto request)
    {
        var response =
            await _stripeService.CreatePaymentIntentAsync(request);

        return Ok(response);
    }

    [HttpGet("verify/{paymentIntentId}")]
    public async Task<IActionResult> VerifyPayment(
        string paymentIntentId)
    {
        var response =
            await _stripeService.VerifyPaymentAsync(paymentIntentId);

        return Ok(response);
    }
}