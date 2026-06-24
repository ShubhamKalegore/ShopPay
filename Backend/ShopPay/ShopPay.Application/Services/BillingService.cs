using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShopPay.Application.DTOs;
using ShopPay.Application.Interfaces;
using ShopPay.Domain.Entities;

namespace ShopPay.Application.Services;

public class BillingService : IBillingService
{
    private readonly IBillingRepository _billingRepository;

    public BillingService(
        IBillingRepository billingRepository)
    {
        _billingRepository = billingRepository;
    }

    public async Task<IEnumerable<BillingDto>> GetAllAsync()
    {
        var billings = await _billingRepository.GetAllAsync();

        return billings.Select(x => new BillingDto
        {
            BillingId = x.BillingId,
            StripePaymentIntentId = x.StripePaymentIntentId,
            OrderId = x.OrderId,
            UserId = x.UserId,
            Amount = x.Amount,
            Currency = x.Currency,
            PaymentStatus = x.PaymentStatus
        });
    }

    public async Task<BillingDto?> GetByIdAsync(
        int billingId)
    {
        var billing = await _billingRepository
            .GetByIdAsync(billingId);

        if (billing == null)
        {
            return null;
        }

        return new BillingDto
        {
            BillingId = billing.BillingId,
            StripePaymentIntentId = billing.StripePaymentIntentId,
            OrderId = billing.OrderId,
            UserId = billing.UserId,
            Amount = billing.Amount,
            Currency = billing.Currency,
            PaymentStatus = billing.PaymentStatus
        };
    }

    public async Task<IEnumerable<BillingDto>> GetByUserIdAsync(
        int userId)
    {
        var billings = await _billingRepository
            .GetByUserIdAsync(userId);

        return billings.Select(x => new BillingDto
        {
            BillingId = x.BillingId,
            StripePaymentIntentId = x.StripePaymentIntentId,
            OrderId = x.OrderId,
            UserId = x.UserId,
            Amount = x.Amount,
            Currency = x.Currency,
            PaymentStatus = x.PaymentStatus
        });
    }

    public async Task<BillingDto?> GetByPaymentIntentIdAsync(
        string stripePaymentIntentId)
    {
        var billing = await _billingRepository
            .GetByPaymentIntentIdAsync(
                stripePaymentIntentId);

        if (billing == null)
        {
            return null;
        }

        return new BillingDto
        {
            BillingId = billing.BillingId,
            StripePaymentIntentId = billing.StripePaymentIntentId,
            OrderId = billing.OrderId,
            UserId = billing.UserId,
            Amount = billing.Amount,
            Currency = billing.Currency,
            PaymentStatus = billing.PaymentStatus
        };
    }

    public async Task<BillingDto> CreateAsync(
        BillingDto billingDto)
    {
        var billing = new Billing
        {
            StripePaymentIntentId =
                billingDto.StripePaymentIntentId,
            OrderId = billingDto.OrderId,
            UserId = billingDto.UserId,
            Amount = billingDto.Amount,
            Currency = billingDto.Currency,
            PaymentStatus = billingDto.PaymentStatus
        };

        await _billingRepository.AddAsync(billing);
        await _billingRepository.SaveChangesAsync();

        billingDto.BillingId = billing.BillingId;

        return billingDto;
    }
}