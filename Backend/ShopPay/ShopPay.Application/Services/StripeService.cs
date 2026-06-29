using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;
using ShopPay.Application.Configurations;
using ShopPay.Application.DTOs.Stripe;
using ShopPay.Application.Interfaces;
using Stripe.Checkout;

namespace ShopPay.Infrastructure.Services;

public class StripeService : IStripeService
{
    private readonly StripeSettings _stripeSettings;

    public StripeService(IOptions<StripeSettings> stripeSettings)
    {
        _stripeSettings = stripeSettings.Value;
    }

    public async Task<CheckoutSessionResponseDto> CreateCheckoutSessionAsync(
        CreateCheckoutSessionRequestDto request)
    {
        var lineItems = request.Items.Select(item => new SessionLineItemOptions
        {
            PriceData = new SessionLineItemPriceDataOptions
            {
                Currency = item.Currency,

                UnitAmountDecimal = item.UnitPrice * 100,

                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = item.ProductName,
                    Description = item.Description
                }
            },

            Quantity = item.Quantity
        }).ToList();

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string>
            {
                "card"
            },

            Mode = "payment",

            SuccessUrl = request.SuccessUrl,

            CancelUrl = request.CancelUrl,

            LineItems = lineItems
        };

        var service = new SessionService();

        var session = await service.CreateAsync(options);

        return new CheckoutSessionResponseDto
        {
            SessionId = session.Id,
            CheckoutUrl = session.Url!
        };
    }

    public async Task<VerifyPaymentResponseDto> VerifyPaymentAsync(
        string sessionId)
    {
        throw new NotImplementedException();
    }
}