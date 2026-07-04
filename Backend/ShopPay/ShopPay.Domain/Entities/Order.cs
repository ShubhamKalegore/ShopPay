using ShopPay.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("orders")]
public class Order
{
    [Key]
    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("shipping_address_id")]
    public int? ShippingAddressId { get; set; }

    [Column("billing_address_id")]
    public int? BillingAddressId { get; set; }

    [Column("total_amount")]
    public decimal TotalAmount { get; set; }

    [Column("is_payment_confirmed")]
    public bool IsPaymentConfirmed { get; set; } = false;

    [Column("stripe_payment_intent_id")]
    [MaxLength(255)]
    public string? StripePaymentIntentId { get; set; }


    [Column("order_status")]
    public string OrderStatus { get; set; } = "PENDING";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [ForeignKey(nameof(ShippingAddressId))]
    public Address? ShippingAddress { get; set; }

    [ForeignKey(nameof(BillingAddressId))]
    public Address? BillingAddress { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }
        = new List<OrderItem>();

    public ICollection<Billing> Billings { get; set; }
    = new List<Billing>();  
}