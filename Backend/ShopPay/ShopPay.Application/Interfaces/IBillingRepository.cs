using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShopPay.Domain.Entities;

namespace ShopPay.Application.Interfaces;

public interface IBillingRepository
{
    Task<IEnumerable<Billing>> GetAllAsync();

    Task<Billing?> GetByIdAsync(int billingId);

    Task<IEnumerable<Billing>> GetByUserIdAsync(int userId);

    Task<Billing?> GetByPaymentIntentIdAsync(
        string stripePaymentIntentId);

    Task AddAsync(Billing billing);

    Task SaveChangesAsync();
}