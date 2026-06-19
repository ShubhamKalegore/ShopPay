using AutoMapper;
using Microsoft.Extensions.Logging;
using ShopPay.Application.DTOs;
using ShopPay.Application.Interfaces;
using ShopPay.Domain.Entities;

namespace ShopPay.Application.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<AddressService> _logger;

    public AddressService(
        IAddressRepository addressRepository,
        IMapper mapper,
        ILogger<AddressService> logger)
    {
        _addressRepository = addressRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<AddressDto>> GetAllAddressesAsync()
    {
        _logger.LogInformation("Fetching all addresses");

        var addresses =
            await _addressRepository.GetAllAsync();

        return _mapper.Map<List<AddressDto>>(addresses);
    }

    public async Task<AddressDto?> GetAddressByIdAsync(int id)
    {
        _logger.LogInformation(
            "Fetching address with id {AddressId}",
            id);

        var address =
            await _addressRepository.GetByIdAsync(id);

        if (address is null)
            return null;

        return _mapper.Map<AddressDto>(address);
    }

    public async Task<AddressDto> CreateAddressAsync(
        AddressDto addressDto)
    {
        _logger.LogInformation(
            "Creating address for user {UserId}",
            addressDto.UserId);

        var now = DateTime.UtcNow;

        var address = new Address
        {
            UserId = addressDto.UserId,
            AddressType = addressDto.AddressType,
            Street = addressDto.Street,
            City = addressDto.City,
            State = addressDto.State,
            PostalCode = addressDto.PostalCode,
            Country = addressDto.Country,
            IsDefault = addressDto.IsDefault,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _addressRepository.AddAsync(address);
        await _addressRepository.SaveChangesAsync();

        return _mapper.Map<AddressDto>(address);
    }

    public async Task<AddressDto?> UpdateAddressAsync(
        int id,
        AddressDto addressDto)
    {
        var address =
            await _addressRepository.GetByIdAsync(id);

        if (address is null)
            return null;

        address.AddressType = addressDto.AddressType;
        address.Street = addressDto.Street;
        address.City = addressDto.City;
        address.State = addressDto.State;
        address.PostalCode = addressDto.PostalCode;
        address.Country = addressDto.Country;
        address.IsDefault = addressDto.IsDefault;
        address.UpdatedAt = DateTime.UtcNow;

        _addressRepository.Update(address);

        await _addressRepository.SaveChangesAsync();

        return _mapper.Map<AddressDto>(address);
    }

    public async Task DeleteAddressAsync(int id)
    {
        var address =
            await _addressRepository.GetByIdAsync(id);

        if (address is null)
            return;

        _addressRepository.Delete(address);

        await _addressRepository.SaveChangesAsync();
    }
}