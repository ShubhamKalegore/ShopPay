using ShopPay.Models;

public interface IUserService
{
    Task<List<User>> GetAllUsersAsyn();
}
