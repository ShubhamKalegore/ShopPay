using ShopPay.Application.Interfaces;
using ShopPay.Domain.Entities;
using ShopPay.Infrastructure.Data;

namespace ShopPay.Infrastructure.Repositories;

public class OrderRepository: GenericRepository<Order, int>, IOrderRepository
{
    public OrderRepository(AppDbContext context)
        : base(context)
    {
    }
}