using ShopPay.Domain.Entities;

namespace ShopPay.Application.Interfaces;

public interface IUserRepository : IGenericRepository<User, int>
{
    Task<User?> GetByEmailAsync(string email);
}
