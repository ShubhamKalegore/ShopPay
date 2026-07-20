using System.ComponentModel;
using ModelContextProtocol.Server;
using ShopPay.Application.DTOs;
using ShopPay.Application.Interfaces;

namespace ShopPay.McpServer.Tools;

[McpServerToolType]
public class OrderTools
{
    private readonly IOrderService _orderService;

    public OrderTools(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [McpServerTool]
    [Description("Returns all orders.")]
    public async Task<IEnumerable<OrderDto>> GetOrders()
    {
        return await _orderService.GetAllOrdersAsync();
    }

    [McpServerTool]
    [Description("Returns an order by its ID.")]
    public async Task<OrderDto?> GetOrder(int id)
    {
        return await _orderService.GetOrderByIdAsync(id);
    }

    [McpServerTool]
    [Description("Returns orders filtered by payment confirmation status. Pass true for paid orders and false for unpaid orders.")]
    public async Task<IEnumerable<OrderDto>> GetOrdersByPaymentStatus(
    bool isPaymentConfirmed)
    {
        return await _orderService.GetOrdersByPaymentStatusAsync(
            isPaymentConfirmed);
    }

    /*    [McpServerTool]
        [Description("Updates an existing order.")]
        public async Task<OrderDto?> UpdateOrder(
            int id,
            OrderDto orderDto)
        {
            return await _orderService.UpdateOrderAsync(id, orderDto);
        }

        [McpServerTool]
        [Description("Deletes an order by its ID.")]
        public async Task DeleteOrder(int id)
        {
            await _orderService.DeleteOrderAsync(id);
        }
    */
}