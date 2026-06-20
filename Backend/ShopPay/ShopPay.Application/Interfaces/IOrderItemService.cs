using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShopPay.Application.DTOs;

namespace ShopPay.Application.Interfaces;

public interface IOrderItemService
{
    Task<List<OrderItemDto>> GetAllOrderItemsAsync();

    Task<OrderItemDto?> GetOrderItemByIdAsync(int id);

    Task<OrderItemDto> CreateOrderItemAsync(OrderItemDto orderItemDto);

    Task<OrderItemDto?> UpdateOrderItemAsync(
        int id,
        OrderItemDto orderItemDto);

    Task DeleteOrderItemAsync(int id);
}