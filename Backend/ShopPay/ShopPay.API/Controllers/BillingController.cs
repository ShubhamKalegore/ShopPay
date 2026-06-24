using Microsoft.AspNetCore.Mvc;

using ShopPay.Application.DTOs;
using ShopPay.Application.Interfaces;

namespace ShopPay.API.Controllers;

[Route("api/billings")]
[ApiController]
public class BillingController : ControllerBase
{
    private readonly IBillingService _billingService;

    public BillingController(
        IBillingService billingService)
    {
        _billingService = billingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var billings =
            await _billingService.GetAllAsync();

        return Ok(billings);
    }

    [HttpGet("{billingId}")]
    public async Task<IActionResult> GetById(
        int billingId)
    {
        var billing =
            await _billingService
                .GetByIdAsync(billingId);

        if (billing == null)
        {
            return NotFound(new
            {
                Message = "Billing record not found."
            });
        }

        return Ok(billing);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(
        int userId)
    {
        var billings =
            await _billingService
                .GetByUserIdAsync(userId);

        return Ok(billings);
    }

    [HttpGet("payment-intent/{paymentIntentId}")]
    public async Task<IActionResult> GetByPaymentIntentId(
        string paymentIntentId)
    {
        var billing =
            await _billingService
                .GetByPaymentIntentIdAsync(
                    paymentIntentId);

        if (billing == null)
        {
            return NotFound(new
            {
                Message = "Billing record not found."
            });
        }

        return Ok(billing);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        BillingDto billingDto)
    {
        var billing =
            await _billingService
                .CreateAsync(billingDto);

        return CreatedAtAction(
            nameof(GetById),
            new
            {
                billingId = billing.BillingId
            },
            billing);
    }
}