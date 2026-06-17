using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ShopPay.Application.DTOs;
using ShopPay.Application.Interfaces;
using ShopPay.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using CleanArchDemo.Application.DTOs;

namespace ShopPay.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly IConfiguration _configuration;

    public UserService(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<UserService> logger,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        _logger.LogInformation("Fetching all users");

        var users = await _userRepository.GetAllAsync();

        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<UserDto> RegisterUserAsync(CreateUserDto createUserDto)
    {
        _logger.LogInformation(
            "Registering user with email {Email}",
            createUserDto.Email);

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

    public async Task<TokenResponseDto?> LoginUserAsync(
        LoginUserDto loginUserDto)
    {
        _logger.LogInformation(
            "Login attempt for email {Email}",
            loginUserDto.Email);

        var user =
            await _userRepository
                .GetByEmailAsync(loginUserDto.Email);

        if (user is null ||
            user.PasswordHash != loginUserDto.Password)
        {
            _logger.LogWarning(
                "Invalid login attempt for email {Email}",
                loginUserDto.Email);

            return null;
        }

        var tokenResponse =
            await CreateTokenResponse(user);

        _logger.LogInformation(
            "Login successful for user id {UserId}",
            user.UserId);

        return tokenResponse;
    }

    public async Task<TokenResponseDto?> RefreshTokensAsync(
        string refreshToken)
    {
        var user =
            await _userRepository
                .GetByRefreshTokenAsync(refreshToken);

        if (user is null)
            return null;

        if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return null;

        return await CreateTokenResponse(user);
    }

    public async Task LogoutAsync(int userId)
    {
        var user =
            await _userRepository
                .GetByIdAsync(userId);

        if (user is null)
            return;

        user.RefreshToken = string.Empty;
        user.RefreshTokenExpiryTime = DateTime.UtcNow;

        await _userRepository.SaveChangesAsync();
    }

    private async Task<TokenResponseDto> CreateTokenResponse(
        User user)
    {
        var accessToken = CreateToken(user);

        var refreshToken =
            await GenerateAndSaveRefreshTokenAsync(user);

        return new TokenResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            UserId = user.UserId
        };
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(
                ClaimTypes.Email,
                user.Email),

            new Claim(
                ClaimTypes.NameIdentifier,
                user.UserId.ToString())
        };

        var key =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["JwtSettings:Secret"]!));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha512);

        var token =
            new JwtSecurityToken(
                issuer:
                    _configuration["JwtSettings:Issuer"],
                audience:
                    _configuration["JwtSettings:Audience"],
                claims: claims,
                expires:
                    DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }

    private async Task<string> GenerateAndSaveRefreshTokenAsync(
        User user)
    {
        var randomNumber = new byte[32];

        using var rng =
            RandomNumberGenerator.Create();

        rng.GetBytes(randomNumber);

        var refreshToken =
            Convert.ToBase64String(randomNumber);

        user.RefreshToken = refreshToken;

        user.RefreshTokenExpiryTime =
            DateTime.UtcNow.AddDays(7);

        await _userRepository.SaveChangesAsync();

        return refreshToken;
    }
}