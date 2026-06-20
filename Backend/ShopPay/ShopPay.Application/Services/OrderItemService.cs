using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using Microsoft.Extensions.Logging;
using ShopPay.Application.DTOs;
using ShopPay.Application.Interfaces;
using ShopPay.Domain.Entities;

namespace ShopPay.Application.Services;

public class OrderItemService : IOrderItemService
{
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderItemService> _logger;

    public OrderItemService(
        IOrderItemRepository orderItemRepository,
        IMapper mapper,
        ILogger<OrderItemService> logger)
    {
        _orderItemRepository = orderItemRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<OrderItemDto>> GetAllOrderItemsAsync()
    {
        var orderItems =
            await _orderItemRepository.GetAllAsync();

        return _mapper.Map<List<OrderItemDto>>(orderItems);
    }

    public async Task<OrderItemDto?> GetOrderItemByIdAsync(int id)
    {
        var orderItem =
            await _orderItemRepository.GetByIdAsync(id);

        if (orderItem is null)
            return null;

        return _mapper.Map<OrderItemDto>(orderItem);
    }

    public async Task<OrderItemDto> CreateOrderItemAsync(
        OrderItemDto orderItemDto)
    {
        var orderItem = new OrderItem
        {
            OrderId = orderItemDto.OrderId,
            ProductId = orderItemDto.ProductId,
            Quantity = orderItemDto.Quantity,
            UnitPrice = orderItemDto.UnitPrice
        };

        await _orderItemRepository.AddAsync(orderItem);
        await _orderItemRepository.SaveChangesAsync();

        return _mapper.Map<OrderItemDto>(orderItem);
    }

    public async Task<OrderItemDto?> UpdateOrderItemAsync(
        int id,
        OrderItemDto orderItemDto)
    {
        var orderItem =
            await _orderItemRepository.GetByIdAsync(id);

        if (orderItem is null)
            return null;

        orderItem.OrderId = orderItemDto.OrderId;
        orderItem.ProductId = orderItemDto.ProductId;
        orderItem.Quantity = orderItemDto.Quantity;
        orderItem.UnitPrice = orderItemDto.UnitPrice;

        _orderItemRepository.Update(orderItem);

        await _orderItemRepository.SaveChangesAsync();

        return _mapper.Map<OrderItemDto>(orderItem);
    }

    public async Task DeleteOrderItemAsync(int id)
    {
        var orderItem =
            await _orderItemRepository.GetByIdAsync(id);

        if (orderItem is null)
            return;

        _orderItemRepository.Delete(orderItem);

        await _orderItemRepository.SaveChangesAsync();
    }
}