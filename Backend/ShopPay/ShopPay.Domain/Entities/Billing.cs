using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopPay.Domain.Entities;

[Table("billing")]
public class Billing
{
    [Key]
    [Column("billing_id")]
    public int BillingId { get; set; }

    [Column("stripe_payment_intent_id")]
    public string StripePaymentIntentId { get; set; } = string.Empty;

    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("currency")]
    public string Currency { get; set; } = "INR";

    [Column("payment_status")]
    public string PaymentStatus { get; set; } = "PENDING";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; } = null!;

    public ICollection<Invoice> Invoices { get; set; }
    = new List<Invoice>();
}