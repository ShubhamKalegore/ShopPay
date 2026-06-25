using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShopPay.Application.DTOs;
using ShopPay.Application.Interfaces;
using ShopPay.Domain.Entities;

namespace ShopPay.Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;

    public InvoiceService(
        IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }

    public async Task<IEnumerable<InvoiceDto>> GetAllAsync()
    {
        var invoices = await _invoiceRepository.GetAllAsync();

        return invoices.Select(x => new InvoiceDto
        {
            InvoiceId = x.InvoiceId,
            StripeInvoiceId = x.StripeInvoiceId,
            StripeInvoiceUrl = x.StripeInvoiceUrl,
            UserId = x.UserId,
            BillingId = x.BillingId,
            SubscriptionId = x.SubscriptionId,
            AmountDue = x.AmountDue,
            AmountPaid = x.AmountPaid,
            Currency = x.Currency,
            InvoiceStatus = x.InvoiceStatus
        });
    }

    public async Task<InvoiceDto?> GetByIdAsync(
        int invoiceId)
    {
        var invoice = await _invoiceRepository
            .GetByIdAsync(invoiceId);

        if (invoice == null)
        {
            return null;
        }

        return new InvoiceDto
        {
            InvoiceId = invoice.InvoiceId,
            StripeInvoiceId = invoice.StripeInvoiceId,
            StripeInvoiceUrl = invoice.StripeInvoiceUrl,
            UserId = invoice.UserId,
            BillingId = invoice.BillingId,
            SubscriptionId = invoice.SubscriptionId,
            AmountDue = invoice.AmountDue,
            AmountPaid = invoice.AmountPaid,
            Currency = invoice.Currency,
            InvoiceStatus = invoice.InvoiceStatus
        };
    }

    public async Task<IEnumerable<InvoiceDto>> GetByUserIdAsync(
        int userId)
    {
        var invoices = await _invoiceRepository
            .GetByUserIdAsync(userId);

        return invoices.Select(x => new InvoiceDto
        {
            InvoiceId = x.InvoiceId,
            StripeInvoiceId = x.StripeInvoiceId,
            StripeInvoiceUrl = x.StripeInvoiceUrl,
            UserId = x.UserId,
            BillingId = x.BillingId,
            SubscriptionId = x.SubscriptionId,
            AmountDue = x.AmountDue,
            AmountPaid = x.AmountPaid,
            Currency = x.Currency,
            InvoiceStatus = x.InvoiceStatus
        });
    }

    public async Task<InvoiceDto?> GetByStripeInvoiceIdAsync(
        string stripeInvoiceId)
    {
        var invoice = await _invoiceRepository
            .GetByStripeInvoiceIdAsync(
                stripeInvoiceId);

        if (invoice == null)
        {
            return null;
        }

        return new InvoiceDto
        {
            InvoiceId = invoice.InvoiceId,
            StripeInvoiceId = invoice.StripeInvoiceId,
            StripeInvoiceUrl = invoice.StripeInvoiceUrl,
            UserId = invoice.UserId,
            BillingId = invoice.BillingId,
            SubscriptionId = invoice.SubscriptionId,
            AmountDue = invoice.AmountDue,
            AmountPaid = invoice.AmountPaid,
            Currency = invoice.Currency,
            InvoiceStatus = invoice.InvoiceStatus
        };
    }

    public async Task<InvoiceDto> CreateAsync(
        InvoiceDto invoiceDto)
    {
        var invoice = new Invoice
        {
            StripeInvoiceId = invoiceDto.StripeInvoiceId,
            StripeInvoiceUrl = invoiceDto.StripeInvoiceUrl,
            UserId = invoiceDto.UserId,
            BillingId = invoiceDto.BillingId,
            SubscriptionId = invoiceDto.SubscriptionId,
            AmountDue = invoiceDto.AmountDue,
            AmountPaid = invoiceDto.AmountPaid,
            Currency = invoiceDto.Currency,
            InvoiceStatus = invoiceDto.InvoiceStatus
        };

        await _invoiceRepository.AddAsync(invoice);
        await _invoiceRepository.SaveChangesAsync();

        invoiceDto.InvoiceId = invoice.InvoiceId;

        return invoiceDto;
    }
}