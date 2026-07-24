using Microsoft.AspNetCore.Mvc;
using ShopPay.Application.DTOs.Stripe;
using ShopPay.Application.Interfaces;
using ShopPay.Application.Services;
using Stripe;

namespace ShopPay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StripeController : ControllerBase
{
    private readonly IStripeService _stripeService;
    private readonly IConfiguration _configuration;
    private readonly IOrderService _orderService;
    private readonly IEmailService _emailService;

    public StripeController(
        IStripeService stripeService, IConfiguration configuration, IOrderService orderService, IEmailService emailService)
    {
        _stripeService = stripeService;
        _configuration = configuration;
        _orderService = orderService;
        _emailService = emailService;
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

                        await _emailService.SendEmailAsync(
                            "shubhamkalegore87@gmail.com",
                            "Thank You for Your Purchase!",
                            @"
                            <h2>Thank You for Your Purchase!</h2>
                            <p>We appreciate you shopping with <strong>ShopPay</strong>.</p>
                            <p>Your order has been received successfully and is being processed.</p>
                            <p>We'll notify you once your order has been shipped.</p>
                            <br/>
                            <p>Thank you for choosing ShopPay. We look forward to serving you again!</p>
                            "
                        );

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