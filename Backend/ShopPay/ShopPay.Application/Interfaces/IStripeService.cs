using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShopPay.Application.DTOs.Stripe;

namespace ShopPay.Application.Interfaces;

public interface IStripeService
{
    /// <summary>
    /// Creates a Stripe Checkout Session and returns the session details.
    /// </summary>
    /// <param name="request">Checkout session request.</param>
    /// <returns>Checkout session response.</returns>
    Task<CheckoutSessionResponseDto> CreateCheckoutSessionAsync(
        CreateCheckoutSessionRequestDto request);

    /// <summary>
    /// Verifies the payment status of a Checkout Session.
    /// </summary>
    /// <param name="sessionId">Stripe Checkout Session Id.</param>
    /// <returns>Payment verification details.</returns>
    Task<VerifyPaymentResponseDto> VerifyPaymentAsync(
        string sessionId);
}