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
    private readonly IOrderService _orderService;

    public StripeController(
        IStripeService stripeService, IConfiguration configuration, IOrderService orderService)
    {
        _stripeService = stripeService;
        _configuration = configuration;
        _orderService = orderService;
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
                    {
                        if (stripeEvent.Data.Object is not PaymentIntent paymentIntent)
                        {
                            throw new Exception("PaymentIntent not found in Stripe event.");
                        }

                        var orderId = await _stripeService.GetOrderIdFromPaymentIntentAsync(
                            paymentIntent.Id ?? throw new Exception("PaymentIntent ID is missing."));

                        if(orderId != null)
                        {
                            await _orderService.UpdateOrderPaymentStatus((int)orderId, true);
                        }

                        break;
                    }

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