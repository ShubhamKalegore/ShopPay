using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using ShopPay.Application.Interfaces;
using ShopPay.Domain.Entities;
using ShopPay.Infrastructure.Data;

namespace ShopPay.Infrastructure.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly AppDbContext _context;

    public SubscriptionRepository(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Subscription>> GetAllAsync()
    {
        return await _context.Subscriptions.ToListAsync();
    }

    public async Task<Subscription?> GetByIdAsync(
        int subscriptionId)
    {
        return await _context.Subscriptions
            .FirstOrDefaultAsync(
                x => x.SubscriptionId == subscriptionId);
    }

    public async Task<IEnumerable<Subscription>> GetByUserIdAsync(
        int userId)
    {
        return await _context.Subscriptions
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task<Subscription?> GetByStripeSubscriptionIdAsync(
        string stripeSubscriptionId)
    {
        return await _context.Subscriptions
            .FirstOrDefaultAsync(
                x => x.StripeSubscriptionId == stripeSubscriptionId);
    }

    public async Task AddAsync(
        Subscription subscription)
    {
        await _context.Subscriptions.AddAsync(subscription);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
