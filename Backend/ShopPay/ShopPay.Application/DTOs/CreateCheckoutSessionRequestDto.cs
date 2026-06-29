using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPay.Application.DTOs.Stripe;

public class CreateCheckoutSessionRequestDto
{
    public int UserId { get; set; }

    public List<CheckoutItemDto> Items { get; set; } = [];

    public string SuccessUrl { get; set; } = string.Empty;

    public string CancelUrl { get; set; } = string.Empty;
}