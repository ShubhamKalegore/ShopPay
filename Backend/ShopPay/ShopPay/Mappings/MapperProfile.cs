using AutoMapper;
using ShopPay.DTOs;
using ShopPay.Models;

namespace ShopPay.Mappings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
