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
    /// Creates a Stripe Payment Intent and returns the client secret.
    /// </summary>
    /// <param name="request">Payment intent request.</param>
    /// <returns>Payment intent details.</returns>
    Task<CreatePaymentIntentResponseDto> CreatePaymentIntentAsync(
        CreatePaymentIntentRequestDto request);

    /// <summary>
    /// Verifies the payment status of a Stripe Payment Intent.
    /// </summary>
    /// <param name="paymentIntentId">Stripe Payment Intent Id.</param>
    /// <returns>Payment verification details.</returns>
    Task<VerifyPaymentResponseDto> VerifyPaymentAsync(string paymentIntentId);

    Task UpdatePaymentIntentOrderIdAsync(string paymentIntentId, int orderId);

    Task<int?> GetOrderIdFromPaymentIntentAsync(string paymentIntentId);
}