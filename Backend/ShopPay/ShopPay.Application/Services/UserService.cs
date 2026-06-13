using AutoMapper;
using Microsoft.Extensions.Logging;
using ShopPay.Application.DTOs;
using ShopPay.Application.Interfaces;
using ShopPay.Domain.Entities;

namespace ShopPay.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        _logger.LogInformation("Fetching all users");

        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<UserDto> RegisterUserAsync(CreateUserDto createUserDto)
    {
        _logger.LogInformation("Registering user with email {Email}", createUserDto.Email);

        var now = DateTime.UtcNow;
        var user = new User
        {
            Email = createUserDto.Email,
            PasswordHash = createUserDto.Password,
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> LoginUserAsync(LoginUserDto loginUserDto)
    {
        _logger.LogInformation("Login attempt for email {Email}", loginUserDto.Email);

        var user = await _userRepository.GetByEmailAsync(loginUserDto.Email);

        if (user is null || user.PasswordHash != loginUserDto.Password)
        {
            _logger.LogWarning("Invalid login attempt for email {Email}", loginUserDto.Email);
            return null;
        }

        return _mapper.Map<UserDto>(user);
    }
}
