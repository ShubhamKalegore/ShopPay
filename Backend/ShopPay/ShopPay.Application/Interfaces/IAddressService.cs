using ShopPay.Application.DTOs;

namespace ShopPay.Application.Interfaces;

public interface IAddressService
{
    Task<List<AddressDto>> GetAllAddressesAsync();

    Task<AddressDto?> GetAddressByIdAsync(int id);
    Task<AddressDto?> GetAddressByUserIdAsync(int userId);

    Task<AddressDto> CreateAddressAsync(AddressDto addressDto);

    Task<AddressDto?> UpdateAddressAsync(int id, AddressDto addressDto);

    Task DeleteAddressAsync(int id);
}