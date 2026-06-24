using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShopPay.Domain.Entities;

namespace ShopPay.Application.Interfaces;

public interface IStripeCustomerRepository
{
    Task<StripeCustomer?> GetByUserIdAsync(int userId);

    Task<StripeCustomer?> GetByStripeCustomerIdAsync(
        string stripeCustomerId);

    Task AddAsync(StripeCustomer stripeCustomer);

    Task SaveChangesAsync();
}