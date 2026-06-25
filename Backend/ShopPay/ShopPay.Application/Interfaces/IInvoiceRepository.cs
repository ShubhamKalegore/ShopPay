using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShopPay.Domain.Entities;

namespace ShopPay.Application.Interfaces;

public interface IInvoiceRepository
{
    Task<IEnumerable<Invoice>> GetAllAsync();

    Task<Invoice?> GetByIdAsync(int invoiceId);

    Task<IEnumerable<Invoice>> GetByUserIdAsync(int userId);

    Task<Invoice?> GetByStripeInvoiceIdAsync(
        string stripeInvoiceId);

    Task AddAsync(Invoice invoice);

    Task SaveChangesAsync();
}