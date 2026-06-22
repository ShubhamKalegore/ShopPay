using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ShopPay.Application.DTOs;
using ShopPay.Application.Interfaces;

namespace ShopPay.API.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAddressService _addressService;

    public UserController(IUserService userService, IAddressService addressService)
    {
        _userService = userService;
        _addressService = addressService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        CreateUserDto createUserDto)
    {
        var user =
            await _userService.RegisterUserAsync(createUserDto);

        return CreatedAtAction(
            nameof(GetUsers),
            new { id = user.UserId },
            user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginUserDto loginUserDto)
    {
        var result =
            await _userService.LoginUserAsync(loginUserDto);

        if (result is null)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        var userAddress = await _addressService.GetAddressByUserIdAsync(result.UserId);

        Response.Cookies.Append(
            "accessToken",
            result.AccessToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            });

        Response.Cookies.Append(
            "refreshToken",
            result.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

        return Ok(new
        {
            userId = result.UserId,
            isAddressPresent = userAddress != null,
            postalCode = userAddress?.PostalCode
        });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken =
            Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(
                "Refresh token missing.");
        }

        var result =
            await _userService
                .RefreshTokensAsync(refreshToken);

        if (result is null)
        {
            return Unauthorized(
                "Invalid refresh token.");
        }

        Response.Cookies.Append(
            "accessToken",
            result.AccessToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            });

        Response.Cookies.Append(
            "refreshToken",
            result.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

        return Ok();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)
                ?.Value;

        if (int.TryParse(userIdClaim, out var userId))
        {
            await _userService.LogoutAsync(userId);
        }

        Response.Cookies.Delete(
            "accessToken",
            new CookieOptions
            {
                Path = "/",
                Secure = true,
                SameSite = SameSiteMode.None
            });

        Response.Cookies.Delete(
            "refreshToken",
            new CookieOptions
            {
                Path = "/",
                Secure = true,
                SameSite = SameSiteMode.None
            });

        return Ok(new
        {
            Message = "Logged out successfully"
        });
    }
}