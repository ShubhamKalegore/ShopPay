using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ShopPay.Application.DTOs.Stripe;

public class VerifyPaymentResponseDto
{
    public string SessionId { get; set; } = string.Empty;

    public string PaymentIntentId { get; set; } = string.Empty;

    public string PaymentStatus { get; set; } = string.Empty;

    public decimal AmountTotal { get; set; }

    public string Currency { get; set; } = string.Empty;

    public bool IsPaid { get; set; }
}
