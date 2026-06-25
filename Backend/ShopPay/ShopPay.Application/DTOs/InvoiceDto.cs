using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPay.Application.DTOs;

public class InvoiceDto
{
    public int InvoiceId { get; set; }

    public string StripeInvoiceId { get; set; }
        = string.Empty;

    public string StripeInvoiceUrl { get; set; }
        = string.Empty;

    public int UserId { get; set; }

    public int? BillingId { get; set; }

    public int? SubscriptionId { get; set; }

    public decimal AmountDue { get; set; }

    public decimal AmountPaid { get; set; }

    public string Currency { get; set; }
        = "INR";

    public string InvoiceStatus { get; set; }
        = string.Empty;
}