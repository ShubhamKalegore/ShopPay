using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;
using ShopPay.Application.DTOs;
using ShopPay.Application.Interfaces;

namespace ShopPay.API.Controllers;

[Route("api/order-items")]
[ApiController]
public class OrderItemController : ControllerBase
{
    private readonly IOrderItemService _orderItemService;

    public OrderItemController(
        IOrderItemService orderItemService)
    {
        _orderItemService = orderItemService;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrderItems()
    {
        var orderItems =
            await _orderItemService.GetAllOrderItemsAsync();

        return Ok(orderItems);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderItem(int id)
    {
        var orderItem =
            await _orderItemService.GetOrderItemByIdAsync(id);

        if (orderItem is null)
        {
            return NotFound(new
            {
                Message = "Order item not found."
            });
        }

        return Ok(orderItem);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrderItem(
        OrderItemDto orderItemDto)
    {
        var orderItem =
            await _orderItemService
                .CreateOrderItemAsync(orderItemDto);

        return CreatedAtAction(
            nameof(GetOrderItem),
            new { id = orderItem.OrderItemId },
            orderItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrderItem(
        int id,
        OrderItemDto orderItemDto)
    {
        var orderItem =
            await _orderItemService
                .UpdateOrderItemAsync(id, orderItemDto);

        if (orderItem is null)
        {
            return NotFound(new
            {
                Message = "Order item not found."
            });
        }

        return Ok(orderItem);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrderItem(int id)
    {
        await _orderItemService.DeleteOrderItemAsync(id);

        return NoContent();
    }
}