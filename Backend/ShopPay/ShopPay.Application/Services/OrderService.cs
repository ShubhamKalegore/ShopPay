using AutoMapper;
using Microsoft.Extensions.Logging;
using ShopPay.Application.DTOs;
using ShopPay.Application.Interfaces;
using ShopPay.Domain.Entities;

namespace ShopPay.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository orderRepository,
        IMapper mapper,
        ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<OrderDto>> GetAllOrdersAsync()
    {
        _logger.LogInformation("Fetching all orders");

        var orders =
            await _orderRepository.GetAllAsync();

        return _mapper.Map<List<OrderDto>>(orders);
    }

    public async Task<OrderDto?> GetOrderByIdAsync(int id)
    {
        _logger.LogInformation(
            "Fetching order with id {OrderId}",
            id);

        var order =
            await _orderRepository.GetByIdAsync(id);

        if (order is null)
            return null;

        return _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto> CreateOrderAsync(
        OrderDto orderDto)
    {
        _logger.LogInformation(
            "Creating order for user {UserId}",
            orderDto.UserId);

        var now = DateTime.UtcNow;

        var order = new Order
        {
            UserId = orderDto.UserId,
            ShippingAddressId = orderDto.ShippingAddressId,
            BillingAddressId = orderDto.BillingAddressId,
            TotalAmount = orderDto.TotalAmount,
            OrderStatus = string.IsNullOrWhiteSpace(orderDto.OrderStatus)
                ? "PENDING"
                : orderDto.OrderStatus,

            IsPaymentConfirmed = false,

            StripePaymentIntentId = orderDto.StripePaymentIntentId,

            CreatedAt = now,
            UpdatedAt = now
        };

        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveChangesAsync();

        return _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto?> UpdateOrderAsync(
        int id,
        OrderDto orderDto)
    {
        var order =
            await _orderRepository.GetByIdAsync(id);

        if (order is null)
            return null;

        order.ShippingAddressId =
            orderDto.ShippingAddressId;

        order.BillingAddressId =
            orderDto.BillingAddressId;

        order.TotalAmount =
            orderDto.TotalAmount;

        order.OrderStatus =
            orderDto.OrderStatus;

        order.UpdatedAt =
            DateTime.UtcNow;

        order.IsPaymentConfirmed =
            orderDto.IsPaymentConfirmed;

        order.StripePaymentIntentId =
            orderDto.StripePaymentIntentId;

        _orderRepository.Update(order);

        await _orderRepository.SaveChangesAsync();

        return _mapper.Map<OrderDto>(order);
    }

    public async Task DeleteOrderAsync(int id)
    {
        var order =
            await _orderRepository.GetByIdAsync(id);

        if (order is null)
            return;

        _orderRepository.Delete(order);

        await _orderRepository.SaveChangesAsync();
    }

    public async Task<List<OrderDto>> GetOrdersByPaymentStatusAsync(bool isPaymentConfirmed)
    {
        _logger.LogInformation(
            "Fetching orders with payment status {Status}",
            isPaymentConfirmed);

        var orders =
            await _orderRepository.GetOrdersByPaymentStatusAsync(
                isPaymentConfirmed);

        return _mapper.Map<List<OrderDto>>(orders);
    }
}