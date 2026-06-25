using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopPay.Domain.Entities;

[Table("invoices")]
public class Invoice
{
    [Key]
    [Column("invoice_id")]
    public int InvoiceId { get; set; }

    [Column("stripe_invoice_id")]
    public string StripeInvoiceId { get; set; } = string.Empty;

    [Column("stripe_invoice_url")]
    public string StripeInvoiceUrl { get; set; } = string.Empty;

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("billing_id")]
    public int? BillingId { get; set; }

    [Column("subscription_id")]
    public int? SubscriptionId { get; set; }

    [Column("amount_due")]
    public decimal AmountDue { get; set; }

    [Column("amount_paid")]
    public decimal AmountPaid { get; set; }

    [Column("currency")]
    public string Currency { get; set; } = "INR";

    [Column("invoice_status")]
    public string InvoiceStatus { get; set; } = "OPEN";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [ForeignKey(nameof(BillingId))]
    public Billing? Billing { get; set; }

    [ForeignKey(nameof(SubscriptionId))]
    public Subscription? Subscription { get; set; }
}
