using AutoMapper;
using ShopPay.Application.DTOs;
using ShopPay.Domain.Entities;

namespace ShopPay.Application.Mappings;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Address, AddressDto>().ReverseMap();
    }
}
