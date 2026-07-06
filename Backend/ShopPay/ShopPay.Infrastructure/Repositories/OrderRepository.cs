using Microsoft.EntityFrameworkCore;
using ShopPay.Application.Interfaces;
using ShopPay.Domain.Entities;
using ShopPay.Infrastructure.Data;

namespace ShopPay.Infrastructure.Repositories;

public class OrderRepository: GenericRepository<Order, int>, IOrderRepository
{
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetOrdersByPaymentStatusAsync(
    bool isPaymentConfirmed)
    {
        return await _context.Orders
            .Where(o => o.IsPaymentConfirmed == isPaymentConfirmed)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ToListAsync();
    }
}