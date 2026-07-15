using Microsoft.AspNetCore.Mvc;
using ShopPay.Application.DTOs.Stripe;
using ShopPay.Application.Interfaces;
using Stripe;

namespace ShopPay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StripeController : ControllerBase
{
    private readonly IStripeService _stripeService;
    private readonly IConfiguration _configuration;

    public StripeController(
        IStripeService stripeService, IConfiguration configuration)
    {
        _stripeService = stripeService;
        _configuration = configuration;
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


    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body)
            .ReadToEndAsync();

        var stripeSignature = Request.Headers["Stripe-Signature"];

        var webhookSecret =
            _configuration["Stripe:WebhookSecret"];

        Event stripeEvent;

        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                json,
                stripeSignature,
                webhookSecret);

            Console.WriteLine($"Webhook received: {stripeEvent.Type}");

            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentSucceeded:

                    var paymentIntent =
                        stripeEvent.Data.Object as PaymentIntent;

                    Console.WriteLine(
                        $"Payment Intent Id: {paymentIntent?.Id}");

                    break;

                case EventTypes.PaymentIntentPaymentFailed:

                    var failedPaymentIntent =
                        stripeEvent.Data.Object as PaymentIntent;

                    break;

                default:

                    break;
            }

            return Ok();

        }
        catch (StripeException ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok();
    }
}