using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using ShopPay.Application.Interfaces;
using ShopPay.Domain.Entities;
using ShopPay.Infrastructure.Data;

namespace ShopPay.Infrastructure.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly AppDbContext _context;

    public InvoiceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Invoice>> GetAllAsync()
    {
        return await _context.Invoices.ToListAsync();
    }

    public async Task<Invoice?> GetByIdAsync(int invoiceId)
    {
        return await _context.Invoices
            .FirstOrDefaultAsync(x => x.InvoiceId == invoiceId);
    }

    public async Task<IEnumerable<Invoice>> GetByUserIdAsync(int userId)
    {
        return await _context.Invoices
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task<Invoice?> GetByStripeInvoiceIdAsync(
        string stripeInvoiceId)
    {
        return await _context.Invoices
            .FirstOrDefaultAsync(
                x => x.StripeInvoiceId == stripeInvoiceId);
    }

    public async Task AddAsync(Invoice invoice)
    {
        await _context.Invoices.AddAsync(invoice);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}