using Microsoft.AspNetCore.Mvc;
using ShopPay.Application.DTOs;
using ShopPay.Application.Interfaces;

namespace ShopPay.API.Controllers;

[Route("api/invoices")]
[ApiController]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(
        IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var invoices = await _invoiceService.GetAllAsync();
        return Ok(invoices);
    }

    [HttpGet("{invoiceId}")]
    public async Task<IActionResult> GetById(
        int invoiceId)
    {
        var invoice =
            await _invoiceService.GetByIdAsync(invoiceId);

        if (invoice == null)
        {
            return NotFound(new
            {
                Message = "Invoice not found."
            });
        }

        return Ok(invoice);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(
        int userId)
    {
        var invoices =
            await _invoiceService.GetByUserIdAsync(userId);

        return Ok(invoices);
    }

    [HttpGet("stripe/{stripeInvoiceId}")]
    public async Task<IActionResult> GetByStripeInvoiceId(
        string stripeInvoiceId)
    {
        var invoice =
            await _invoiceService
                .GetByStripeInvoiceIdAsync(
                    stripeInvoiceId);

        if (invoice == null)
        {
            return NotFound(new
            {
                Message = "Invoice not found."
            });
        }

        return Ok(invoice);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        InvoiceDto invoiceDto)
    {
        var invoice =
            await _invoiceService
                .CreateAsync(invoiceDto);

        return CreatedAtAction(
            nameof(GetById),
            new
            {
                invoiceId = invoice.InvoiceId
            },
            invoice);
    }
}