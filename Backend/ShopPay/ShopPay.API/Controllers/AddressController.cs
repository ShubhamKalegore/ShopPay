using Microsoft.AspNetCore.Mvc;
using ShopPay.Application.DTOs;
using ShopPay.Application.Interfaces;

namespace ShopPay.API.Controllers;

[Route("api/addresses")]
[ApiController]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressController(
        IAddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAddresses()
    {
        var addresses =
            await _addressService.GetAllAddressesAsync();

        return Ok(addresses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAddress(int id)
    {
        var address =
            await _addressService.GetAddressByIdAsync(id);

        if (address is null)
        {
            return NotFound(new
            {
                Message = "Address not found."
            });
        }

        return Ok(address);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAddress(
        AddressDto addressDto)
    {
        var address =
            await _addressService.CreateAddressAsync(addressDto);

        return CreatedAtAction(
            nameof(GetAddress),
            new { id = address.AddressId },
            address);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAddress(
        int id,
        AddressDto addressDto)
    {
        var address =
            await _addressService.UpdateAddressAsync(
                id,
                addressDto);

        if (address is null)
        {
            return NotFound(new
            {
                Message = "Address not found."
            });
        }

        return Ok(address);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAddress(int id)
    {
        await _addressService.DeleteAddressAsync(id);

        return NoContent();
    }
}