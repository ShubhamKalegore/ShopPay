using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopPay.Domain.Entities;

[Table("subscriptions")]
public class Subscription
{
    [Key]
    [Column("subscription_id")]
    public int SubscriptionId { get; set; }

    [Column("stripe_subscription_id")]
    public string StripeSubscriptionId { get; set; } = string.Empty;

    [Column("stripe_price_id")]
    public string StripePriceId { get; set; } = string.Empty;

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("plan_name")]
    public string PlanName { get; set; } = string.Empty;

    [Column("status")]
    public string Status { get; set; } = "ACTIVE";

    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Column("end_date")]
    public DateTime? EndDate { get; set; }

    [Column("auto_renew")]
    public bool AutoRenew { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    public ICollection<Invoice> Invoices { get; set; }
        = new List<Invoice>();
}