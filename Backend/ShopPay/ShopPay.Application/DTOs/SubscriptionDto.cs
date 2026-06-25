using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPay.Application.DTOs;

public class SubscriptionDto
{
    public int SubscriptionId { get; set; }

    public string StripeSubscriptionId { get; set; }
        = string.Empty;

    public string StripePriceId { get; set; }
        = string.Empty;

    public int UserId { get; set; }

    public string PlanName { get; set; }
        = string.Empty;

    public string Status { get; set; }
        = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool AutoRenew { get; set; }
}