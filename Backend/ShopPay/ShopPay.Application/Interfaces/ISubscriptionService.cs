using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShopPay.Application.DTOs;

namespace ShopPay.Application.Interfaces;

public interface ISubscriptionService
{
    Task<IEnumerable<SubscriptionDto>> GetAllAsync();

    Task<SubscriptionDto?> GetByIdAsync(
        int subscriptionId);

    Task<IEnumerable<SubscriptionDto>> GetByUserIdAsync(
        int userId);

    Task<SubscriptionDto?> GetByStripeSubscriptionIdAsync(
        string stripeSubscriptionId);

    Task<SubscriptionDto> CreateAsync(
        SubscriptionDto subscriptionDto);
}