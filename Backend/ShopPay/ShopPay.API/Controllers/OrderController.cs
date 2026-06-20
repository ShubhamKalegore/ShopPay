using Microsoft.AspNetCore.Mvc;
using ShopPay.Application.DTOs;
using ShopPay.Application.Interfaces;

namespace ShopPay.API.Controllers;

[Route("api/orders")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(
        IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders =
            await _orderService.GetAllOrdersAsync();

        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var order =
            await _orderService.GetOrderByIdAsync(id);

        if (order is null)
        {
            return NotFound(new
            {
                Message = "Order not found."
            });
        }

        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(
        OrderDto orderDto)
    {
        var order =
            await _orderService.CreateOrderAsync(orderDto);

        return CreatedAtAction(
            nameof(GetOrder),
            new { id = order.OrderId },
            order);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(
        int id,
        OrderDto orderDto)
    {
        var order =
            await _orderService.UpdateOrderAsync(
                id,
                orderDto);

        if (order is null)
        {
            return NotFound(new
            {
                Message = "Order not found."
            });
        }

        return Ok(order);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        await _orderService.DeleteOrderAsync(id);

        return NoContent();
    }
}