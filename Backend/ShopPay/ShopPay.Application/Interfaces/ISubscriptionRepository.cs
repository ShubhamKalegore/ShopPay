using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShopPay.Domain.Entities;

namespace ShopPay.Application.Interfaces;

public interface ISubscriptionRepository
{
    Task<IEnumerable<Subscription>> GetAllAsync();

    Task<Subscription?> GetByIdAsync(int subscriptionId);

    Task<IEnumerable<Subscription>> GetByUserIdAsync(int userId);

    Task<Subscription?> GetByStripeSubscriptionIdAsync(
        string stripeSubscriptionId);

    Task AddAsync(Subscription subscription);

    Task SaveChangesAsync();
}