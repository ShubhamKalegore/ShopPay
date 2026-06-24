using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPay.Application.DTOs;

public class StripeCustomerDto
{
    public string StripeCustomerId { get; set; } = string.Empty;

    public int UserId { get; set; }
}