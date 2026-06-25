using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShopPay.Application.DTOs;
using ShopPay.Application.Interfaces;
using ShopPay.Domain.Entities;

namespace ShopPay.Application.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    public SubscriptionService(
        ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<IEnumerable<SubscriptionDto>> GetAllAsync()
    {
        var subscriptions =
            await _subscriptionRepository.GetAllAsync();

        return subscriptions.Select(x => new SubscriptionDto
        {
            SubscriptionId = x.SubscriptionId,
            StripeSubscriptionId = x.StripeSubscriptionId,
            StripePriceId = x.StripePriceId,
            UserId = x.UserId,
            PlanName = x.PlanName,
            Status = x.Status,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            AutoRenew = x.AutoRenew
        });
    }

    public async Task<SubscriptionDto?> GetByIdAsync(
        int subscriptionId)
    {
        var subscription =
            await _subscriptionRepository.GetByIdAsync(subscriptionId);

        if (subscription == null)
        {
            return null;
        }

        return new SubscriptionDto
        {
            SubscriptionId = subscription.SubscriptionId,
            StripeSubscriptionId = subscription.StripeSubscriptionId,
            StripePriceId = subscription.StripePriceId,
            UserId = subscription.UserId,
            PlanName = subscription.PlanName,
            Status = subscription.Status,
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            AutoRenew = subscription.AutoRenew
        };
    }

    public async Task<IEnumerable<SubscriptionDto>> GetByUserIdAsync(
        int userId)
    {
        var subscriptions =
            await _subscriptionRepository.GetByUserIdAsync(userId);

        return subscriptions.Select(x => new SubscriptionDto
        {
            SubscriptionId = x.SubscriptionId,
            StripeSubscriptionId = x.StripeSubscriptionId,
            StripePriceId = x.StripePriceId,
            UserId = x.UserId,
            PlanName = x.PlanName,
            Status = x.Status,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            AutoRenew = x.AutoRenew
        });
    }

    public async Task<SubscriptionDto?> GetByStripeSubscriptionIdAsync(
        string stripeSubscriptionId)
    {
        var subscription =
            await _subscriptionRepository
                .GetByStripeSubscriptionIdAsync(
                    stripeSubscriptionId);

        if (subscription == null)
        {
            return null;
        }

        return new SubscriptionDto
        {
            SubscriptionId = subscription.SubscriptionId,
            StripeSubscriptionId = subscription.StripeSubscriptionId,
            StripePriceId = subscription.StripePriceId,
            UserId = subscription.UserId,
            PlanName = subscription.PlanName,
            Status = subscription.Status,
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            AutoRenew = subscription.AutoRenew
        };
    }

    public async Task<SubscriptionDto> CreateAsync(
        SubscriptionDto subscriptionDto)
    {
        var subscription = new Subscription
        {
            StripeSubscriptionId = subscriptionDto.StripeSubscriptionId,
            StripePriceId = subscriptionDto.StripePriceId,
            UserId = subscriptionDto.UserId,
            PlanName = subscriptionDto.PlanName,
            Status = subscriptionDto.Status,
            StartDate = subscriptionDto.StartDate,
            EndDate = subscriptionDto.EndDate,
            AutoRenew = subscriptionDto.AutoRenew
        };

        await _subscriptionRepository.AddAsync(subscription);
        await _subscriptionRepository.SaveChangesAsync();

        subscriptionDto.SubscriptionId = subscription.SubscriptionId;

        return subscriptionDto;
    }
}