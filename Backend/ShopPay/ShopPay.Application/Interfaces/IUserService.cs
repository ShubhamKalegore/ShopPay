using CleanArchDemo.Application.DTOs;
using ShopPay.Application.DTOs;

namespace ShopPay.Application.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetAllUsersAsync();

    Task<UserDto> RegisterUserAsync(CreateUserDto createUserDto);

    Task<TokenResponseDto?> LoginUserAsync(LoginUserDto loginUserDto);

    Task<TokenResponseDto?> RefreshTokensAsync(string refreshToken);

    Task LogoutAsync(int userId);
}
