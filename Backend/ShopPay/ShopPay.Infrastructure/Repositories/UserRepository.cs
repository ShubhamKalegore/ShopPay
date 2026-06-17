using Microsoft.EntityFrameworkCore;
using ShopPay.Application.Interfaces;
using ShopPay.Domain.Entities;
using ShopPay.Infrastructure.Data;

namespace ShopPay.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User, int>, IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        return _context.Users
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public Task<User?> GetByIdAsync(int id)
    {
        return _context.Users
            .FirstOrDefaultAsync(x => x.UserId == id);
    }


    public Task<User?> GetByRefreshTokenAsync(string refreshToken)
    {
        return _context.Users
            .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}