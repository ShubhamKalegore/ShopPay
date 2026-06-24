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

public class BillingRepository : IBillingRepository
{
    private readonly AppDbContext _context;

    public BillingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Billing>> GetAllAsync()
    {
        return await _context.Billings.ToListAsync();
    }

    public async Task<Billing?> GetByIdAsync(int billingId)
    {
        return await _context.Billings
            .FirstOrDefaultAsync(x => x.BillingId == billingId);
    }

    public async Task<IEnumerable<Billing>> GetByUserIdAsync(int userId)
    {
        return await _context.Billings
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task<Billing?> GetByPaymentIntentIdAsync(
        string stripePaymentIntentId)
    {
        return await _context.Billings
            .FirstOrDefaultAsync(
                x => x.StripePaymentIntentId == stripePaymentIntentId);
    }

    public async Task AddAsync(Billing billing)
    {
        await _context.Billings.AddAsync(billing);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}