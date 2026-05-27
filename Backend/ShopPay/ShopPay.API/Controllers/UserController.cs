using Microsoft.AspNetCore.Mvc;
using ShopPay.Application.DTOs;
using ShopPay.Application.Interfaces;

namespace ShopPay.API.Controllers
{
	[Route("api/users")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet]
		public async Task<IActionResult> GetUsers()
		{
			var users = await _userService.GetAllUsersAsync();
			return Ok(users);
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(CreateUserDto createUserDto)
		{
			var user = await _userService.RegisterUserAsync(createUserDto);
			return CreatedAtAction(nameof(GetUsers), new { id = user.UserId }, user);
		}
	}
}
