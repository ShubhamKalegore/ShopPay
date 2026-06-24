using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShopPay.Application.DTOs;

namespace ShopPay.Application.Interfaces;

public interface IStripeCustomerService
{
    Task<StripeCustomerDto?> GetByUserIdAsync(
        int userId);

    Task<StripeCustomerDto?> GetByStripeCustomerIdAsync(
        string stripeCustomerId);

    Task<StripeCustomerDto> CreateAsync(
        StripeCustomerDto stripeCustomerDto);
}