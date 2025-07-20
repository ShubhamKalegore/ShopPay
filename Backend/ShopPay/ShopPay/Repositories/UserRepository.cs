using ShopPay.Models;
using ShopPay.Repositories.Interfaces;

namespace ShopPay.Repositories
{
    public class UserRepository : GenericRepository<User, int>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }
    }
}
