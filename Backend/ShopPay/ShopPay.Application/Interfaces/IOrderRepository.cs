using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShopPay.Domain.Entities;

namespace ShopPay.Application.Interfaces;

public interface IOrderRepository : IGenericRepository<Order, int>
{
    Task<List<Order>> GetOrdersByPaymentStatusAsync(bool isPaymentConfirmed);
}
