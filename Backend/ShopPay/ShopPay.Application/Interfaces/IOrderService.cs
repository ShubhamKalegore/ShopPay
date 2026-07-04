using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShopPay.Application.DTOs;

namespace ShopPay.Application.Interfaces;

public interface IOrderService
{
    Task<List<OrderDto>> GetAllOrdersAsync();

    Task<OrderDto?> GetOrderByIdAsync(int id);

    Task<OrderDto> CreateOrderAsync(OrderDto orderDto);

    Task<OrderDto?> UpdateOrderAsync(int id, OrderDto orderDto);

    Task DeleteOrderAsync(int id);

    Task<List<OrderDto>> GetOrdersByPaymentStatusAsync(bool isPaymentConfirmed);
}