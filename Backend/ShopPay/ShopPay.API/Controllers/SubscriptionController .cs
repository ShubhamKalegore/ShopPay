    using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;
using ShopPay.Application.DTOs;
using ShopPay.Application.Interfaces;

namespace ShopPay.API.Controllers;

[Route("api/subscriptions")]
[ApiController]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionController(
        ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var subscriptions =
            await _subscriptionService.GetAllAsync();

        return Ok(subscriptions);
    }

    [HttpGet("{subscriptionId}")]
    public async Task<IActionResult> GetById(
        int subscriptionId)
    {
        var subscription =
            await _subscriptionService
                .GetByIdAsync(subscriptionId);

        if (subscription == null)
        {
            return NotFound(new
            {
                Message = "Subscription not found."
            });
        }

        return Ok(subscription);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(
        int userId)
    {
        var subscriptions =
            await _subscriptionService
                .GetByUserIdAsync(userId);

        return Ok(subscriptions);
    }

    [HttpGet("stripe/{stripeSubscriptionId}")]
    public async Task<IActionResult> GetByStripeSubscriptionId(
        string stripeSubscriptionId)
    {
        var subscription =
            await _subscriptionService
                .GetByStripeSubscriptionIdAsync(
                    stripeSubscriptionId);

        if (subscription == null)
        {
            return NotFound(new
            {
                Message = "Subscription not found."
            });
        }

        return Ok(subscription);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        SubscriptionDto subscriptionDto)
    {
        var subscription =
            await _subscriptionService
                .CreateAsync(subscriptionDto);

        return CreatedAtAction(
            nameof(GetById),
            new
            {
                subscriptionId = subscription.SubscriptionId
            },
            subscription);
    }
}
