namespace ShopPay.Application.DTOs;

public class OrderDto
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public int? ShippingAddressId { get; set; }

    public int? BillingAddressId { get; set; }

    public decimal TotalAmount { get; set; }

    public string OrderStatus { get; set; } = string.Empty;

    public bool IsPaymentConfirmed { get; set; }

    public string? StripePaymentIntentId { get; set; }

    public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
}