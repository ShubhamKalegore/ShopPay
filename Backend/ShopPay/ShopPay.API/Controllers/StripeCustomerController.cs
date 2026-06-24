using Microsoft.AspNetCore.Mvc;
using ShopPay.Application.DTOs;
using ShopPay.Application.Interfaces;

namespace ShopPay.API.Controllers;

[Route("api/stripe-customers")]
[ApiController]
public class StripeCustomerController : ControllerBase
{
    private readonly IStripeCustomerService _stripeCustomerService;

    public StripeCustomerController(
        IStripeCustomerService stripeCustomerService)
    {
        _stripeCustomerService = stripeCustomerService;
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(
        int userId)
    {
        var customer =
            await _stripeCustomerService
                .GetByUserIdAsync(userId);

        if (customer == null)
        {
            return NotFound(new
            {
                Message = "Stripe customer not found."
            });
        }

        return Ok(customer);
    }

    [HttpGet("{stripeCustomerId}")]
    public async Task<IActionResult> GetByStripeCustomerId(
        string stripeCustomerId)
    {
        var customer =
            await _stripeCustomerService
                .GetByStripeCustomerIdAsync(
                    stripeCustomerId);

        if (customer == null)
        {
            return NotFound(new
            {
                Message = "Stripe customer not found."
            });
        }

        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        StripeCustomerDto stripeCustomerDto)
    {
        var customer =
            await _stripeCustomerService
                .CreateAsync(stripeCustomerDto);

        return CreatedAtAction(
            nameof(GetByStripeCustomerId),
            new
            {
                stripeCustomerId =
                    customer.StripeCustomerId
            },
            customer);
    }
}