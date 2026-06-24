using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShopPay.Application.DTOs;

namespace ShopPay.Application.Interfaces;

public interface IBillingService
{
    Task<IEnumerable<BillingDto>> GetAllAsync();

    Task<BillingDto?> GetByIdAsync(
        int billingId);

    Task<IEnumerable<BillingDto>> GetByUserIdAsync(
        int userId);

    Task<BillingDto?> GetByPaymentIntentIdAsync(
        string stripePaymentIntentId);

    Task<BillingDto> CreateAsync(
        BillingDto billingDto);
}