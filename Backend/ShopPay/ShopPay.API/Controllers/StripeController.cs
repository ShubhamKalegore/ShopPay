using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;
using ShopPay.Application.DTOs.Stripe;
using ShopPay.Application.Interfaces;

namespace ShopPay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StripeController : ControllerBase
{
    private readonly IStripeService _stripeService;

    public StripeController(IStripeService stripeService)
    {
        _stripeService = stripeService;
    }

    [HttpPost("create-checkout-session")]
    public async Task<IActionResult> CreateCheckoutSession(
        [FromBody] CreateCheckoutSessionRequestDto request)
    {
        var response =
            await _stripeService.CreateCheckoutSessionAsync(request);

        return Ok(response);
    }

    [HttpGet("verify/{sessionId}")]
    public async Task<IActionResult> VerifyPayment(
        string sessionId)
    {
        var response =
            await _stripeService.VerifyPaymentAsync(sessionId);

        return Ok(response);
    }
}
