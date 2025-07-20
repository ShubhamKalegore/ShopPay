using ShopPay.Models;
using ShopPay.Repositories.Interfaces;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<User>> GetAllUsersAsyn()
    {
        var users = await _userRepository.GetAllAsync();
        return users.ToList();
    }
}
