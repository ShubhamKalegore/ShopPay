using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPay.Application.DTOs.Stripe;

public class CreatePaymentIntentRequestDto
{
    public List<PaymentItemDto> Items { get; set; } = [];
}