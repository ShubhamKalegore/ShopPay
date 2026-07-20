using ModelContextProtocol.Server;
using ShopPay.Application.DTOs;
using ShopPay.Application.Interfaces;
using System.ComponentModel;

namespace ShopPay.McpServer.Tools;

[McpServerToolType]
public class AddressTools
{
    private readonly IAddressService _addressService;

    public AddressTools(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [McpServerTool]
    [Description("Returns all addresses.")]
    public async Task<IEnumerable<AddressDto>> GetAddresses()
    {
        return await _addressService.GetAllAddressesAsync();
    }

    [McpServerTool]
    [Description("Returns the address with the specified ID.")]
    public async Task<AddressDto?> GetAddress(int id)
    {
        return await _addressService.GetAddressByIdAsync(id);
    }

    [McpServerTool]
    [Description("Creates a new address.")]
    public async Task<AddressDto> CreateAddress(AddressDto addressDto)
    {
        return await _addressService.CreateAddressAsync(addressDto);
    }

    [McpServerTool]
    [Description("Updates an existing address.")]
    public async Task<AddressDto?> UpdateAddress(
        int id,
        AddressDto addressDto)
    {
        return await _addressService.UpdateAddressAsync(id, addressDto);
    }

    [McpServerTool]
    [Description("Deletes the address with the specified ID.")]
    public async Task DeleteAddress(int id)
    {
        await _addressService.DeleteAddressAsync(id);
    }
}