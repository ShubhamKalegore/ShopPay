using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPay.Domain.Enums;

public enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    PaymentFailed = 2,
    Cancelled = 3,
    Shipped = 4,
    Delivered = 5
}