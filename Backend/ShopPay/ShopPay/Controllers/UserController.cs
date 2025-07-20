
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopPay.DTOs;

namespace ShopPay.Controllers
{
	[Route("api/users")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IMapper _mapper;

		public UserController(IUserService userService, IMapper mapper)
		{
			_userService = userService;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> GetUsers()
		{
			var users = await _userService.GetAllUsersAsyn();
			var userDtos = _mapper.Map<List<UserDto>>(users);
			return Ok(userDtos);
		}
	}
}
