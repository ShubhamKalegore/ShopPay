using ShopPay.Application.DTOs.Stripe;
using ShopPay.Application.Interfaces;
using Stripe;

namespace ShopPay.Infrastructure.Services;

public class StripeService : IStripeService
{
    public async Task<CreatePaymentIntentResponseDto> CreatePaymentIntentAsync(
        CreatePaymentIntentRequestDto request)
    {
        var totalAmount = request.Items.Sum(item =>
            item.UnitPrice * item.Quantity);

        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(totalAmount * 100),

            Currency = request.Items.First().Currency,

            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true
            }
        };

        var service = new PaymentIntentService();

        var paymentIntent =
            await service.CreateAsync(options);

        return new CreatePaymentIntentResponseDto
        {
            ClientSecret = paymentIntent.ClientSecret!,
            PaymentIntentId = paymentIntent.Id
        };
    }

    public async Task<VerifyPaymentResponseDto> VerifyPaymentAsync(
        string paymentIntentId)
    {
        var service = new PaymentIntentService();

        var paymentIntent =
            await service.GetAsync(paymentIntentId);

        return new VerifyPaymentResponseDto
        {
            PaymentIntentId = paymentIntent.Id,
            PaymentStatus = paymentIntent.Status,
            AmountTotal = paymentIntent.Amount / 100m,
            Currency = paymentIntent.Currency,
            IsPaid = paymentIntent.Status == "succeeded"
        };
    }
}