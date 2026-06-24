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

public class StripeCustomerRepository : IStripeCustomerRepository
{
    private readonly AppDbContext _context;

    public StripeCustomerRepository(
        AppDbContext context)
    {
        _context = context;
    }

    public Task<StripeCustomer?> GetByUserIdAsync(
        int userId)
    {
        return _context.StripeCustomers
            .FirstOrDefaultAsync(
                x => x.UserId == userId);
    }

    public Task<StripeCustomer?> GetByStripeCustomerIdAsync(
        string stripeCustomerId)
    {
        return _context.StripeCustomers
            .FirstOrDefaultAsync(
                x => x.StripeCustomerId == stripeCustomerId);
    }

    public async Task AddAsync(
        StripeCustomer stripeCustomer)
    {
        await _context.StripeCustomers
            .AddAsync(stripeCustomer);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}