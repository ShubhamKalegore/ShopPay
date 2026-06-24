using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShopPay.Application.DTOs;
using ShopPay.Application.Interfaces;
using ShopPay.Domain.Entities;

namespace ShopPay.Application.Services;

public class StripeCustomerService : IStripeCustomerService
{
    private readonly IStripeCustomerRepository
        _stripeCustomerRepository;

    public StripeCustomerService(
        IStripeCustomerRepository stripeCustomerRepository)
    {
        _stripeCustomerRepository =
            stripeCustomerRepository;
    }

    public async Task<StripeCustomerDto?> GetByUserIdAsync(
        int userId)
    {
        var customer =
            await _stripeCustomerRepository
                .GetByUserIdAsync(userId);

        if (customer == null)
        {
            return null;
        }

        return new StripeCustomerDto
        {
            StripeCustomerId =
                customer.StripeCustomerId,
            UserId =
                customer.UserId
        };
    }

    public async Task<StripeCustomerDto?>
        GetByStripeCustomerIdAsync(
            string stripeCustomerId)
    {
        var customer =
            await _stripeCustomerRepository
                .GetByStripeCustomerIdAsync(
                    stripeCustomerId);

        if (customer == null)
        {
            return null;
        }

        return new StripeCustomerDto
        {
            StripeCustomerId =
                customer.StripeCustomerId,
            UserId =
                customer.UserId
        };
    }

    public async Task<StripeCustomerDto>
        CreateAsync(
            StripeCustomerDto stripeCustomerDto)
    {
        var customer = new StripeCustomer
        {
            StripeCustomerId =
                stripeCustomerDto
                    .StripeCustomerId,
            UserId =
                stripeCustomerDto.UserId
        };

        await _stripeCustomerRepository
            .AddAsync(customer);

        await _stripeCustomerRepository
            .SaveChangesAsync();

        return stripeCustomerDto;
    }
}
