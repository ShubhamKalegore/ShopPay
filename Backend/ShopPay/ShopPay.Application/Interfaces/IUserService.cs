using ShopPay.Application.DTOs;

namespace ShopPay.Application.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetAllUsersAsync();
    Task<UserDto> RegisterUserAsync(CreateUserDto createUserDto);
}
