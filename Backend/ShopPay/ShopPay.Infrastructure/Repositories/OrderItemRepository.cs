using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopPay.Application.Interfaces;
using ShopPay.Domain.Entities;
using ShopPay.Infrastructure.Data;

namespace ShopPay.Infrastructure.Repositories;

public class OrderItemRepository
    : GenericRepository<OrderItem, int>,
      IOrderItemRepository
{
    public OrderItemRepository(AppDbContext context)
        : base(context)
    {
    }
}
