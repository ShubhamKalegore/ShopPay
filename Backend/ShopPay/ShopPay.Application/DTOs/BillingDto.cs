using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPay.Application.DTOs;

public class BillingDto
{
    public int BillingId { get; set; }

    public string StripePaymentIntentId { get; set; }
        = string.Empty;

    public int OrderId { get; set; }

    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; }
        = "INR";

    public string PaymentStatus { get; set; }
        = string.Empty;
}