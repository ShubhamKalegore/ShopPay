using Microsoft.EntityFrameworkCore;
using ShopPay.Application.Interfaces;
using ShopPay.Domain.Entities;
using ShopPay.Infrastructure.Data;

namespace ShopPay.Infrastructure.Repositories;

public class AddressRepository
    : GenericRepository<Address, int>,
      IAddressRepository
{
    private readonly AppDbContext _context;
    public AddressRepository(AppDbContext context)
        : base(context)
    {
        _context = context;
    }
    public async Task<Address?> GetAddressByUserIdAsync(int userId)
    {
        return await _context.Address
            .FirstOrDefaultAsync(a => a.UserId == userId);
    }

}