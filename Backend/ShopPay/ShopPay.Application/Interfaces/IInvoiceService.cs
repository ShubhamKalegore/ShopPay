using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShopPay.Application.DTOs;

namespace ShopPay.Application.Interfaces;

public interface IInvoiceService
{
    Task<IEnumerable<InvoiceDto>> GetAllAsync();

    Task<InvoiceDto?> GetByIdAsync(
        int invoiceId);

    Task<IEnumerable<InvoiceDto>> GetByUserIdAsync(
        int userId);

    Task<InvoiceDto?> GetByStripeInvoiceIdAsync(
        string stripeInvoiceId);

    Task<InvoiceDto> CreateAsync(
        InvoiceDto invoiceDto);
}
